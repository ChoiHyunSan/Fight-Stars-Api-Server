using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using StackExchange.Redis;

namespace Domain.Game.Services;

public class MatchManager
{
    private readonly IConnectionMultiplexer _redis;
    private readonly RoomDispatcher _roomDispatcher;

    private static readonly Dictionary<long, WebSocket> _userSockets = new();
    private static readonly object _lock = new();

    // 모드별 매칭 인원 수 설정
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

    public async Task EnqueueAsync(MatchRequest request, WebSocket socket)
    {
        var db = _redis.GetDatabase();
        var normalizedMode = request.Mode.ToLower();
        var queueKey = $"match:{normalizedMode}";

        Console.WriteLine($"[MATCH] 요청 수신: userId={request.UserId}, mode={normalizedMode}");

        // Redis Set에 userId 추가 (중복 방지)
        var added = await db.SetAddAsync(queueKey, request.UserId);
        Console.WriteLine($"[MATCH] Redis SetAdd {queueKey} 결과: {added}");

        if (!added)
        {
            Console.WriteLine($"[MATCH] userId {request.UserId} 이미 큐에 있음. 무시");
            return;
        }

        // WebSocket 저장
        lock (_lock)
        {
            _userSockets[request.UserId] = socket;
        }

        // TTL 설정
        var ttlKey = $"match:user:{request.UserId}";
        await db.StringSetAsync(ttlKey, "queued", TimeSpan.FromSeconds(30));
        Console.WriteLine($"[MATCH] TTL 설정 완료: {ttlKey}");

        // 전체 유저 목록 확인
        var allUserIds = (await db.SetMembersAsync(queueKey))
            .Select(v => (long)v)
            .ToList();

        Console.WriteLine($"[MATCH] {queueKey} 현재 유저 수: {allUserIds.Count}");

        var validUserIds = new List<long>();
        foreach (var uid in allUserIds)
        {
            bool exists = await db.KeyExistsAsync($"match:user:{uid}");
            Console.WriteLine($"[MATCH] TTL 상태 확인: match:user:{uid} → exists? {exists}");

            if (exists)
            {
                validUserIds.Add(uid);
                if (validUserIds.Count >= MatchRequirements[normalizedMode])
                    break;
            }
            else
            {
                await db.SetRemoveAsync(queueKey, uid);
                Console.WriteLine($"[MATCH] TTL 만료 → 큐에서 제거: {uid}");
            }
        }

        if (validUserIds.Count < MatchRequirements[normalizedMode])
        {
            Console.WriteLine($"[MATCH] 유효 유저 부족 → 대기 중: {validUserIds.Count}/{MatchRequirements[normalizedMode]}");
            return;
        }

        Console.WriteLine($"[MATCH] 매칭 조건 충족 → 방 생성 시도");

        foreach (var uid in validUserIds)
        {
            await db.SetRemoveAsync(queueKey, uid);
            await db.KeyDeleteAsync($"match:user:{uid}");
        }

        var roomInfo = await _roomDispatcher.CreateRoomAsync(validUserIds, normalizedMode);

        var resultJson = JsonSerializer.Serialize(new
        {
            eventType = "matchSuccess",
            data = roomInfo
        });

        var buffer = Encoding.UTF8.GetBytes(resultJson);
        var segment = new ArraySegment<byte>(buffer);

        foreach (var uid in validUserIds)
        {
            lock (_lock)
            {
                if (_userSockets.TryGetValue(uid, out var ws))
                {
                    Console.WriteLine($"[MATCH] matchSuccess 전송 시도: uid={uid}, 소켓 상태={ws.State}");

                    if (ws.State == WebSocketState.Open)
                    {
                        ws.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                    }

                    _userSockets.Remove(uid);
                }
                else
                {
                    Console.WriteLine($"[MATCH] 소켓 없음 → 전송 실패: uid={uid}");
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
                db.SetRemoveAsync($"match:{mode}", userId);
                db.KeyDeleteAsync($"match:user:{userId}");
            }
        }
    }
}
