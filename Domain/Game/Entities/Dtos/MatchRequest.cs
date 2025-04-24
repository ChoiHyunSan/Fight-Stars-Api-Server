public class MatchRequest
{
    public long UserId { get; set; }
    public int CharacterId { get; set; }
    public string Mode { get; set; } = "deathmatch"; // 기본값
}