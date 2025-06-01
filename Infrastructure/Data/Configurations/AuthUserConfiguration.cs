using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FightStars_ApiServer.Infrastructure.Data.Configurations
{
    public class AuthUserConfiguration : IEntityTypeConfiguration<AuthUser>
    {
        public void Configure(EntityTypeBuilder<AuthUser> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Username).IsRequired();
            builder.Property(a => a.Email).IsRequired();
            builder.HasIndex(a => a.Email).IsUnique();
        }
    }
}
