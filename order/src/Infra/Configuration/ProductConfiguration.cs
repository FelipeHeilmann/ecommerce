using Domain.Products.Entity;
using Domain.Products.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(200).HasColumnName("name");
        builder.Property(p => p.Description).HasMaxLength(250).HasColumnName("description");
        builder.Property(p => p.Sku).HasConversion(sku => sku.Value, value => Sku.Create(value).Value).HasColumnName("sku");
        builder.Property(p => p.CreatedAt).HasColumnName("created_at"); 
        builder.Property(p => p.ImageUrl).HasColumnName("image_url");
        builder.Property(p => p.CategoryId).HasColumnName("category_id");

        builder.OwnsOne(p => p.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Currency).HasMaxLength(3).HasColumnName("price_currency");
            priceBuilder.Property(m => m.Amount).HasColumnName("price_amount");
        });

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId);

        builder.ToTable("products");
    }
}
