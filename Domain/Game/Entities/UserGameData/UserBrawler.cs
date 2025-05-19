/***************************
        UserBrawler
***************************/
// Description
// : GameUserId - 유저 ID (FK)
// : BrawlerId - 브롤러 ID (FK)
// : Level - 브롤러 레벨
// : Trophy - 브롤러 트로피 수치
// : PowerPoint - 브롤러 강화 포인트
// Author : ChoiHyunSan

public class UserBrawler
{
    public long GameUserId { get; set; }
    public int BrawlerId { get; set; }
    public int Level { get; set; } = 1;
    public int Trophy { get; set; } = 0;
    public int PowerPoint { get; set; } = 0;
    public GameUser? GameUser { get; set; }
    public Brawler? Brawler { get; set; }

    public static UserBrawler Create(GameUser gameUser, Brawler brawler)
    {
        return new UserBrawler
        {
            GameUserId = gameUser.Id,
            Brawler = brawler,
            PowerPoint = 0,
            Trophy = 0,
            BrawlerId = brawler.Id,
            GameUser = gameUser,
            Level = 1
        };
    }
}