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

    public DbSet<AuthUser> AuthUser { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }
    public DbSet<GameUser> GameUsers { get; set; }
    public DbSet<UserCurrency> UserCurrencies { get; set; }
    public DbSet<UserStats> UserStats { get; set; }
    public DbSet<UserInventory> UserInventories { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Skin> Skins { get; set; }
    public DbSet<UserSkin> UserSkins { get; set; }
    public DbSet<Brawler> Brawlers { get; set; }
    public DbSet<UserBrawler> UserBrawlers { get; set; }
    public DbSet<UserBattleHistory> UserBattleHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}