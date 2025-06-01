using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FightStars_ApiServer.Infrastructure.Data.Configurations
{
    public class SkinConfiguration : IEntityTypeConfiguration<Skin>
    {
        public void Configure(EntityTypeBuilder<Skin> builder)
        {
            builder.HasKey(s => s.Id);
            builder.HasOne<Brawler>()
                   .WithMany()
                   .HasForeignKey(s => s.BrawlerId);
        }
    }
}
