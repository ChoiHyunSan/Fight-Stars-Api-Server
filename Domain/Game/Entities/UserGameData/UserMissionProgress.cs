/***************************
   UserMissionProgress
***************************/
// Description
// : GameUserId - 유저 ID (FK)
// : MissionId - 미션 ID (FK)
// : Progress - 현재 진행 수치
// : IsCompleted - 완료 여부
// Author : ChoiHyunSan
public class UserMissionProgress
{
    public long GameUserId { get; set; }
    public int MissionId { get; set; }
    public int Progress { get; set; } = 0;
    public bool IsCompleted { get; set; } = false;

    public GameUser? GameUser { get; set; }
    public Mission? Mission { get; set; }
}