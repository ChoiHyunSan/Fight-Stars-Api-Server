/***************************
        UserCurrency
***************************/
// Description
// : GameUserId - 소유 유저의 ID (PK)
// : Gold - 일반 재화
// : Gems - 프리미엄 재화
// : Energy - 플레이를 위한 에너지
// : Exp - 경험치
// Author : ChoiHyunSan
using System.ComponentModel.DataAnnotations;

public class UserCurrency
{
    [Key]
    public long GameUserId { get; set; }
    public int Gold { get; set; } = 0;
    public int Gems { get; set; } = 0;
    public int Energy { get; set; } = 0;
    public int Exp { get; set; } = 0;

    public GameUser? GameUser { get; set; }
}
