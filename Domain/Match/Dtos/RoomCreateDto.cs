public class RoomCreateRequest
{
    public string Mode { get; set; }
    public List<long> UserIds { get; set; } = new List<long>();
}

public class RoomCreateResponse
{
    public string roomId { get; set; }
    public string password { get; set; }
    public string ip { get; set; }
    public int port { get; set; }
}