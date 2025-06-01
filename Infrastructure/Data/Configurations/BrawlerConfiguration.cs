using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FightStars_ApiServer.Infrastructure.Data.Configurations
{
    public class BrawlerConfiguration : IEntityTypeConfiguration<Brawler>
    {
        public void Configure(EntityTypeBuilder<Brawler> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name).IsRequired();
        }
    }
}
