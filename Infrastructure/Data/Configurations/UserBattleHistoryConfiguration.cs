using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FightStars_ApiServer.Infrastructure.Data.Configurations
{
    public class UserBattleHistoryConfiguration : IEntityTypeConfiguration<UserBattleHistory>
    {
        public void Configure(EntityTypeBuilder<UserBattleHistory> builder)
        {
            builder.HasKey(ubh => ubh.Id);
            builder.HasOne(ubh => ubh.GameUser)
                   .WithMany(g => g.BattleHistories)
                   .HasForeignKey(ubh => ubh.GameUserId);
        }
    }
}
