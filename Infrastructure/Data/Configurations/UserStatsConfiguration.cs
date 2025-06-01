using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FightStars_ApiServer.Infrastructure.Data.Configurations
{
    public class UserStatsConfiguration : IEntityTypeConfiguration<UserStats>
    {
        public void Configure(EntityTypeBuilder<UserStats> builder)
        {
            builder.HasKey(u => u.GameUserId);
            builder.HasOne(u => u.GameUser)
                   .WithOne(g => g.Stats)
                   .HasForeignKey<UserStats>(u => u.GameUserId);
        }
    }
}
