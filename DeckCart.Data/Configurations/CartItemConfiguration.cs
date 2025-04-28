using DeckCart.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeckCart.Data.Configurations
{    public class CartItemConfiguration : IEntityTypeConfiguration<CartEntity>
    {
        public void Configure(EntityTypeBuilder<CartEntity> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.HasOne(ci => ci.User)
                   .WithMany(u => u.CartItems)
                   .HasForeignKey(ci => ci.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.Item)
                   .WithMany()
                   .HasForeignKey(ci => ci.ItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
