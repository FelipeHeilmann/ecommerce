using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
{
    public void Configure(EntityTypeBuilder<LineItem> builder)
    {
        builder.HasKey(li => li.Id);
        builder.Property(li => li.Id).HasColumnName("id");
        builder.Property(li => li.OrderId).HasColumnName("order_id");
        builder.Property(li => li.ProductId).HasColumnName("product_id");
        builder.Property(li => li.Quantity).HasColumnName("quantity");

        builder.OwnsOne(li => li.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Currency).HasMaxLength(3).HasColumnName("price_currency");
            priceBuilder.Property(m => m.Amount).HasColumnName("price_amount");
        });

        builder.ToTable("line_items");
    }
}
