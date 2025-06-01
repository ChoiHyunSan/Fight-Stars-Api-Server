using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FightStars_ApiServer.Infrastructure.Data.Configurations
{
    public class UserInventoryConfiguration : IEntityTypeConfiguration<UserInventory>
    {
        public void Configure(EntityTypeBuilder<UserInventory> builder)
        {
            builder.HasKey(ui => new { ui.GameUserId, ui.ItemId });
            builder.HasOne(ui => ui.GameUser)
                   .WithMany(g => g.Inventories)
                   .HasForeignKey(ui => ui.GameUserId);
            builder.HasOne(ui => ui.Item)
                   .WithMany(i => i.UserInventories)
                   .HasForeignKey(ui => ui.ItemId);
        }
    }
}
