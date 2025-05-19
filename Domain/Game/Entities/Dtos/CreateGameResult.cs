public class CreateGameResultRequest
{
    public  List<PlayerGameResultData> PlayerGameResults { get; set; } = new();
}

public class PlayerGameResultData
{
    public int PlayerId { get; set; }
    public bool IsWin { get; set; }
    public int Exp { get; set; }
    public int Gold { get; set; }
}

public class CreateGameResultResponse
{
    public List<UserGameResultData> UserGameResults { get; set; } = new();
}

public class UserGameResultData
{
    public long UserId { get; set; }
    public int WinCount { get; set; } = 0;
    public int LoseCount { get; set; } = 0;
    public int TotalPlayCount { get; set; } = 0;
    public int Gold { get; set; } = 0;
    public int Energy { get; set; } = 0;
    public int Exp { get; set; } = 0;
    public int Level { get; set; } = 1;
}