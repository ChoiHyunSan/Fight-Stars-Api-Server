using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<AuthUser> AuthUser { get; set; } 
    public DbSet<RefreshToken> RefreshToken { get; set; } 
}
