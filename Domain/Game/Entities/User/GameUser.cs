/***************************
          GameUser
***************************/
// Description
// : Id - 고유 유저 ID
// : AccountId - 인증 계정의 참조 ID
// : Nickname - 게임 내 닉네임
// : Avatar - 프로필 아바타 경로 또는 아이디
// : CreatedAt - 계정 생성 일시
// : LastLoginAt - 마지막 로그인 일시
// : IsBanned - 정지 여부
// Author : ChoiHyunSan

public class GameUser
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
    public bool IsBanned { get; set; } = false;

    public UserCurrency Currency { get; set; }
    public UserStats? Stats { get; set; }
    public List<UserInventory> Inventories { get; set; } = new();
    public List<UserBrawler> Brawlers { get; set; } = new();
    public List<UserSkin> Skins { get; set; } = new();
    public List<UserBattleHistory> BattleHistories { get; set; } = new();
    public void UpdateGameResult(PlayerGameResultData info)
    {
        Stats.TotalPlayCount++;
        Stats.WinCount += info.IsWin ? 1 : 0;
        Stats.LoseCount += !info.IsWin ? 1 : 0;
        Currency.Gold += info.Gold;
    
        // 경험치를 토대로 레벨업이 된다.
        Currency.Exp += info.Exp;
        const int levelUpExp = 400;
        if (Currency.Exp >= levelUpExp)
        {
            Stats.Level += Currency.Exp / levelUpExp;
            Currency.Exp %= levelUpExp;
        }
        
        Currency.Energy -= 0; // TODO: 게임 결과에 따라 에너지 차감 로직 추가 필요
    }
}