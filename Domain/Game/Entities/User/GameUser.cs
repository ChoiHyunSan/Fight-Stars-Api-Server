﻿/***************************
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

    public UserCurrency? Currency { get; set; }
    public UserStats? Stats { get; set; }
    public List<UserInventory> Inventories { get; set; } = new();
    public List<UserBrawler> Brawlers { get; set; } = new();
    public List<UserBattleHistory> BattleHistories { get; set; } = new();
    public List<UserMissionProgress> MissionProgresses { get; set; } = new();
}