/***************************
         UserStats
***************************/
// Description
// : GameUserId - 소유 유저의 ID (PK)
// : Level - 유저 레벨
// : WinCount - 승리 횟수
// : LoseCount - 패배 횟수
// : TotalPlayCount - 전체 게임 수
// : HighestRank - 도달한 최고 랭크
// : CurrentTrophy - 현재 트로피 점수
// Author : ChoiHyunSan
using System.ComponentModel.DataAnnotations;

public class UserStats
{
    [Key]
    public long GameUserId { get; set; }
    public int Level { get; set; } = 1;
    public int WinCount { get; set; } = 0;
    public int LoseCount { get; set; } = 0;
    public int TotalPlayCount { get; set; } = 0;
    public int HighestRank { get; set; } = 0;
    public int CurrentTrophy { get; set; } = 0;

    public GameUser? GameUser { get; set; }
}