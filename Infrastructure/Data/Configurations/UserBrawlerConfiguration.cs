using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FightStars_ApiServer.Infrastructure.Data.Configurations
{
    public class UserBrawlerConfiguration : IEntityTypeConfiguration<UserBrawler>
    {
        public void Configure(EntityTypeBuilder<UserBrawler> builder)
        {
            builder.HasKey(ub => new { ub.GameUserId, ub.BrawlerId });
            builder.HasOne(ub => ub.GameUser)
                   .WithMany(g => g.Brawlers)
                   .HasForeignKey(ub => ub.GameUserId);
            builder.HasOne(ub => ub.Brawler)
                   .WithMany(b => b.UserBrawlers)
                   .HasForeignKey(ub => ub.BrawlerId);
        }
    }
}
