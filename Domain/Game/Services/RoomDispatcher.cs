using System.Text.Json;
using System.Text;

public class RoomDispatcher
{
    private readonly HttpClient _httpClient;

    public RoomDispatcher()
    {
        _httpClient = new HttpClient(); // 필요시 IHttpClientFactory로 개선 가능
    }

    public async Task<object> CreateRoomAsync(List<long> userIds, string mode)
    {
        var json = JsonSerializer.Serialize(new
        {
            userIds,
            mode
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // 게임 서버의 방 생성 API 호출
        var response = await _httpClient.PostAsync("http://localhost:5005/create-room", content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<object>(result)!;
    }
}