using System.Text.Json;
using System.Text;

public class RoomDispatcher
{
    private readonly HttpClient _httpClient;

    public RoomDispatcher()
    {
        _httpClient = new HttpClient();
    }

    public async Task<RoomCreateResponse> CreateRoomAsync(List<long> userIds, string mode)
    {
        var request = new RoomCreateRequest
        {
            Mode = mode,
            UserIds = userIds
        };
        var json = JsonSerializer.Serialize(request);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // 게임 서버의 방 생성 API 호출
        var response = await _httpClient.PostAsync("http://localhost:5005/create-room", content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        Console.Write($"CreateRoomAsync Result: {result}");

        return JsonSerializer.Deserialize<RoomCreateResponse>(result)!;
    }
}