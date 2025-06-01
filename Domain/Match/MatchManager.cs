using StackExchange.Redis;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;

public class MatchManager
{
    private readonly IConnectionMultiplexer _redis;
    private readonly RoomDispatcher _roomDispatcher;

    private static readonly Dictionary<long, WebSocket> _userSockets = new();
    private static readonly object _lock = new();

    public static readonly Dictionary<string, int> MatchRequirements = new()
    {
        { "deathmatch", 1 },
        { "occupation", 1 },
    };

    public MatchManager(IConnectionMultiplexer redis, RoomDispatcher roomDispatcher)
    {
        _redis = redis;
        _roomDispatcher = roomDispatcher;
    }

    // Redis Key 생성 메서드
    private static string GetQueueKey(string mode) => $"match:{mode.ToLower()}";
    private static string GetTTLKey(long userId) => $"match:user:{userId}";
    private static string GetUserInfoKey(long userId) => $"match:userinfo:{userId}";

    // WebSocket 등록
    private void RegisterSocket(long userId, WebSocket socket)
    {
        lock (_lock)
        {
            _userSockets[userId] = socket;
        }
    }

    // WebSocket 제거
    private void RemoveSocket(long userId)
    {
        lock (_lock)
        {
            _userSockets.Remove(userId);
        }
    }

    // Redis 유저 등록 처리
    private async Task RegisterUserInRedis(IDatabase db, string queueKey, MatchRequest request)
    {
        await db.SetAddAsync(queueKey, request.UserId);
        await db.StringSetAsync(GetTTLKey(request.UserId), "queued", TimeSpan.FromSeconds(30));
        var userInfoJson = JsonSerializer.Serialize(new UserGameInfo
        {
            UserId = request.UserId,
            CharacterId = request.CharacterId,
            SkinId = request.SkinId
        });
        await db.StringSetAsync(GetUserInfoKey(request.UserId), userInfoJson, TimeSpan.FromSeconds(30));
    }

    public async Task EnqueueAsync(MatchRequest request, WebSocket socket)
    {
        var db = _redis.GetDatabase();
        var mode = request.Mode.ToLower();
        var queueKey = GetQueueKey(mode);

        Console.WriteLine($"[MATCH] 요청 수신: userId={request.UserId}, mode={mode}");

        if (!await db.SetAddAsync(queueKey, request.UserId))
        {
            Console.WriteLine($"[MATCH] userId {request.UserId} 이미 큐에 있음. 무시");
            return;
        }

        RegisterSocket(request.UserId, socket);
        await RegisterUserInRedis(db, queueKey, request);

        var validUsers = await GetValidUsers(db, queueKey, mode);
        if (validUsers.Count < MatchRequirements[mode])
        {
            Console.WriteLine($"[MATCH] 유효 유저 부족 → 대기 중: {validUsers.Count}/{MatchRequirements[mode]}");
            return;
        }

        Console.WriteLine($"[MATCH] 매칭 조건 충족 → 방 생성 시도");

        await CleanupMatchedUsers(db, queueKey, validUsers);
        await NotifyMatchSuccess(validUsers, await _roomDispatcher.CreateRoomAsync(validUsers, mode));
    }

    private async Task<List<UserGameInfo>> GetValidUsers(IDatabase db, string queueKey, string mode)
    {
        var allUserIds = (await db.SetMembersAsync(queueKey)).Select(v => (long)v).ToList();
        var validUsers = new List<UserGameInfo>();

        foreach (var uid in allUserIds)
        {
            if (!await db.KeyExistsAsync(GetTTLKey(uid)))
            {
                await db.SetRemoveAsync(queueKey, uid);
                Console.WriteLine($"[MATCH] TTL 만료 → 큐에서 제거: {uid}");
                continue;
            }

            var userInfoJson = await db.StringGetAsync(GetUserInfoKey(uid));
            if (!userInfoJson.IsNullOrEmpty)
            {
                var userInfo = JsonSerializer.Deserialize<UserGameInfo>(userInfoJson);
                if (userInfo != null)
                {
                    validUsers.Add(userInfo);
                    if (validUsers.Count >= MatchRequirements[mode]) break;
                }
            }
        }
        return validUsers;
    }

    private async Task CleanupMatchedUsers(IDatabase db, string queueKey, List<UserGameInfo> users)
    {
        foreach (var user in users)
        {
            await db.SetRemoveAsync(queueKey, user.UserId);
            await db.KeyDeleteAsync(GetTTLKey(user.UserId));
            await db.KeyDeleteAsync(GetUserInfoKey(user.UserId));
        }
    }

    private async Task NotifyMatchSuccess(List<UserGameInfo> users, RoomCreateResponse roomInfo)
    {
        var resultJson = JsonSerializer.Serialize(new MatchResponse
        {
            roomId = roomInfo.roomId,
            password = roomInfo.password,
            ip = roomInfo.ip,
            port = roomInfo.port
        });
        var buffer = Encoding.UTF8.GetBytes(resultJson);
        var segment = new ArraySegment<byte>(buffer);

        foreach (var user in users)
        {
            lock (_lock)
            {
                if (_userSockets.TryGetValue(user.UserId, out var ws) && ws.State == WebSocketState.Open)
                {
                    ws.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                    _userSockets.Remove(user.UserId);
                }
            }
        }
    }

    public async Task HandleDisconnection(WebSocket socket)
    {
        lock (_lock)
        {
            var match = _userSockets.FirstOrDefault(x => x.Value == socket);
            if (match.Key == 0) return;

            var userId = match.Key;
            _userSockets.Remove(userId);

            var db = _redis.GetDatabase();
            foreach (var mode in MatchRequirements.Keys)
            {
                db.SetRemoveAsync(GetQueueKey(mode), userId);
                db.KeyDeleteAsync(GetTTLKey(userId));
                db.KeyDeleteAsync(GetUserInfoKey(userId));
            }
        }
    }
}
