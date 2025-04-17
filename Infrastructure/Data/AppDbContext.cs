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
}
