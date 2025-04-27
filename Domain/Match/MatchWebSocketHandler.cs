using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Domain.Game.Services;

public class MatchWebSocketHandler
{
    private readonly MatchManager _matchManager;

    public MatchWebSocketHandler(MatchManager matchManager)
    {
        _matchManager = matchManager;
    }

    public async Task HandleAsync(WebSocket socket)
    {
        var buffer = new byte[1024 * 4];

        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var json = Encoding.UTF8.GetString(buffer, 0, result.Count);

                try
                {
                    var request = JsonSerializer.Deserialize<MatchRequest>(json);
                    if (request != null)
                    {
                        // TODO : 유저 검증 로직 추가

                        await _matchManager.EnqueueAsync(request, socket);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[WebSocketHandler] JSON 파싱 오류: {ex.Message}");
                }
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await _matchManager.HandleDisconnection(socket);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                break;
            }
        }
    }
}
