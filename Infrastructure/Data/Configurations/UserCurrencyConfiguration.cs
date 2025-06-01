using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FightStars_ApiServer.Infrastructure.Data.Configurations
{
    public class UserCurrencyConfiguration : IEntityTypeConfiguration<UserCurrency>
    {
        public void Configure(EntityTypeBuilder<UserCurrency> builder)
        {
            builder.HasKey(u => u.GameUserId);
            builder.HasOne(u => u.GameUser)
                   .WithOne(g => g.Currency)
                   .HasForeignKey<UserCurrency>(u => u.GameUserId);
        }
    }
}
