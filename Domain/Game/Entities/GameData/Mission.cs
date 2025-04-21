/***************************
          Mission
***************************/
// Description
// : Id - 미션 고유 ID
// : Description - 미션 설명
// : GoalType - 미션 목표 타입 (ex: win_count)
// : GoalValue - 목표 달성 수치
// Author : ChoiHyunSan

public class Mission
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string GoalType { get; set; } = string.Empty;
    public int GoalValue { get; set; }

    public List<UserMissionProgress> UserProgresses { get; set; } = new();
}