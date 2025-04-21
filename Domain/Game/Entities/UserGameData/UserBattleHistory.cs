/***************************
     UserBattleHistory
***************************/
// Description
// : Id - 전투 기록 고유 ID
// : GameUserId - 유저 ID (FK)
// : BattleType - 전투 유형 (solo, team 등)
// : Result - 전투 결과 (win, lose)
// : TrophyChange - 트로피 변동 수치
// : Timestamp - 전투 일시
// Author : ChoiHyunSan
public class UserBattleHistory
{
    public long Id { get; set; }
    public long GameUserId { get; set; }
    public string BattleType { get; set; } = "solo";
    public string Result { get; set; } = "win";
    public int TrophyChange { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public GameUser? GameUser { get; set; }
}