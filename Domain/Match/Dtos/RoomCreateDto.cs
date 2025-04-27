public class RoomCreateRequest
{
    public string Mode { get; set; }
    public List<UserGameInfo> UserInfos { get; set; } = new List<UserGameInfo>();
}

public class UserGameInfo
{
    public long UserId { get; set; }
    public int CharacterId { get; set; }
    public int SkinId { get; set; }
}

public class RoomCreateResponse
{
    public string roomId { get; set; }
    public string password { get; set; }
    public string ip { get; set; }
    public int port { get; set; }
}