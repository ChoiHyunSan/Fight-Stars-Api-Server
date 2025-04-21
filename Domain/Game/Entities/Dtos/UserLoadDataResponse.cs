/***************************
   UserLoadDataResponse
***************************/
// Description
// : 유저가 로그인 후 로딩화면에서 불러올 전체 데이터를 담는 DTO
// Author : ChoiHyunSan
public class UserLoadDataResponse
{
    public string Nickname { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public UserCurrencyDto Currency { get; set; } = new();
    public UserStatsDto Stats { get; set; } = new();
    public List<UserInventoryDto> Inventory { get; set; } = new();
    public List<UserBrawlerDto> Brawlers { get; set; } = new();
}

public class UserCurrencyDto
{
    public int Gold { get; set; }
    public int Gems { get; set; }
    public int Energy { get; set; }
    public int Exp { get; set; }
}

public class UserStatsDto
{
    public int Level { get; set; }
    public int WinCount { get; set; }
    public int LoseCount { get; set; }
    public int TotalPlayCount { get; set; }
    public int HighestRank { get; set; }
    public int CurrentTrophy { get; set; }
}

public class UserInventoryDto
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}

public class UserBrawlerDto
{
    public int BrawlerId { get; set; }
    public int Level { get; set; }
    public int Trophy { get; set; }
    public int PowerPoint { get; set; }
}
