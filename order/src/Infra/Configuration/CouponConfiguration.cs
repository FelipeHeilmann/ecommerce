using Infra.Models.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class CouponConfiguration : IEntityTypeConfiguration<CouponModel>
{
    public void Configure(EntityTypeBuilder<CouponModel> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.Name).HasColumnName("name");
        builder.Property(c => c.Value).HasColumnName("value");
        builder.Property(c => c.ExpiressAt).HasColumnName("expiress_at");

        builder.ToTable("coupons");
    }
}
