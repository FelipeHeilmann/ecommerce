using Domain.Categories.Entity;
using Infra.Models.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryModel>
{
    public void Configure(EntityTypeBuilder<CategoryModel> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.Name).HasMaxLength(150).HasColumnName("name");
        builder.Property(c => c.Description).HasMaxLength(200).HasColumnName("description");
      
        builder.ToTable("categories");
    }
}
