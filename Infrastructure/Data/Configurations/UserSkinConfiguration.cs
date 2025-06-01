using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FightStars_ApiServer.Infrastructure.Data.Configurations
{
    public class UserSkinConfiguration : IEntityTypeConfiguration<UserSkin>
    {
        public void Configure(EntityTypeBuilder<UserSkin> builder)
        {
            builder.HasKey(us => new { us.GameUserId, us.SkinId });
            builder.HasOne(us => us.GameUser)
                   .WithMany(g => g.Skins)
                   .HasForeignKey(us => us.GameUserId);
            builder.HasOne(us => us.Skin)
                   .WithMany(s => s.UserSkins)
                   .HasForeignKey(us => us.SkinId);
        }
    }
}
