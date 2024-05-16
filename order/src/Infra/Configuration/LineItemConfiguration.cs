using Domain.Products.Entity;
using Infra.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class LineItemConfiguration : IEntityTypeConfiguration<LineItemModel>
{
    public void Configure(EntityTypeBuilder<LineItemModel> builder)
    {
        builder.HasKey(li => li.Id);
        builder.Property(li => li.Id).HasColumnName("id");
        builder.Property(li => li.OrderId).HasColumnName("order_id");
        builder.Property(li => li.ProductId).HasColumnName("product_id");
        builder.Property(li => li.Quantity).HasColumnName("quantity");

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(p => p.ProductId);

        builder.Property(m => m.Currency).HasMaxLength(3).HasColumnName("price_currency");
        builder.Property(m => m.Amount).HasColumnName("price_amount");

        builder.ToTable("line_items");
    }
}
