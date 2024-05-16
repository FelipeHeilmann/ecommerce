using Infra.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<ProductsModel>
{
    public void Configure(EntityTypeBuilder<ProductsModel> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(200).HasColumnName("name");
        builder.Property(p => p.Description).HasMaxLength(250).HasColumnName("description");
        builder.Property(p => p.Sku).HasColumnName("sku");
        builder.Property(p => p.CreatedAt).HasColumnName("created_at"); 
        builder.Property(p => p.ImageUrl).HasColumnName("image_url");
        builder.Property(p => p.CategoryId).HasColumnName("category_id");

        builder.Property(m => m.Currency).HasMaxLength(3).HasColumnName("price_currency");
        builder.Property(m => m.Amount).HasColumnName("price_amount");

        builder.HasOne<CategoryModel>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId);

        builder.ToTable("products");
    }
}
