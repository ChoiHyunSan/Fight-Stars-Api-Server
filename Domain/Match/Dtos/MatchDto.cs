public class MatchRequest
{
    public long UserId { get; set; }

    public string JwtToken { get; set; } = string.Empty;
    public int CharacterId { get; set; }

    public int SkinId { get; set; }
    public string Mode { get; set; } = "deathmatch"; // 기본값
}

public class MatchResponse
{
    public string roomId { get; set; }
    public string password { get; set; }
    public string ip { get; set; }
    public int port { get; set; }
}