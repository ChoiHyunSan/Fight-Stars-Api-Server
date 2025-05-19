using Microsoft.EntityFrameworkCore;

/***************************

        AppDbContext

***************************/
// Description
// : DbContext를 상속받은 클래스입니다.
//   데이터베이스와의 연결을 관리합니다.
//   ORM (객체 관계 매핑)을 사용하여 데이터베이스와 상호작용합니다.
//   DbSet<T> 속성을 사용하여 엔티티를 정의합니다.
//
// Author : ChoiHyunSan
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Auth Entities
    public DbSet<AuthUser> AuthUser { get; set; } 
    public DbSet<RefreshToken> RefreshToken { get; set; }

    // Game Entities
    public DbSet<GameUser> GameUsers => Set<GameUser>();
    public DbSet<UserCurrency> UserCurrencies => Set<UserCurrency>();
    public DbSet<UserStats> UserStats => Set<UserStats>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Skin> Skins => Set<Skin>();
    public DbSet<UserInventory> UserInventories => Set<UserInventory>();
    public DbSet<Brawler> Brawlers => Set<Brawler>();
    public DbSet<UserBrawler> UserBrawlers => Set<UserBrawler>();
    public DbSet<UserSkin> UserSkins => Set<UserSkin>();
    public DbSet<UserBattleHistory> UserBattleHistories => Set<UserBattleHistory>();
    public DbSet<Mission> Missions => Set<Mission>();
    public DbSet<UserMissionProgress> UserMissionProgresses => Set<UserMissionProgress>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 복합키 설정
        modelBuilder.Entity<UserInventory>()
            .HasKey(x => new { x.GameUserId, x.ItemId });

        modelBuilder.Entity<UserBrawler>()
            .HasKey(x => new { x.GameUserId, x.BrawlerId });

        modelBuilder.Entity<UserSkin>()
            .HasKey(x => new { x.GameUserId, x.SkinId });

        modelBuilder.Entity<UserMissionProgress>()
            .HasKey(x => new { x.GameUserId, x.MissionId });

        modelBuilder.Entity<UserSkin>()
            .HasOne(us => us.Skin)
            .WithMany(s => s.UserSkins)
            .HasForeignKey(us => us.SkinId);
    }
}
