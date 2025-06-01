using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FightStars_ApiServer.Infrastructure.Data.Configurations
{
    public class GameUserConfiguration : IEntityTypeConfiguration<GameUser>
    {
        public void Configure(EntityTypeBuilder<GameUser> builder)
        {
            builder.HasKey(g => g.Id);
            builder.HasIndex(g => g.AccountId).IsUnique();
        }
    }
}
