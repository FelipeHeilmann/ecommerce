using Infra.Models.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<OrderModel>
{
    public void Configure(EntityTypeBuilder<OrderModel> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("id");
        builder.Property(o => o.CustomerId).HasColumnName("customer_id");
        builder.Property(o => o.Status).HasColumnName("status");
        builder.Property(o => o.CreatedAt).HasColumnName("created_at");
        builder.Property(o => o.UpdatedAt).HasColumnName("updated_at");
        builder.Property(o => o.BillingAddressId).HasColumnName("billing_address_id");
        builder.Property(o => o.ShippingAddressId).HasColumnName("shipping_address_id");

        builder.HasMany(o => o.Items)
             .WithOne()
             .HasForeignKey(li => li.OrderId);

        builder.HasOne<CustomerModel>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        builder.HasOne<AddressModel>()
            .WithMany()
            .HasForeignKey(o => o.BillingAddressId);

        builder.HasOne<AddressModel>()
            .WithMany()
            .HasForeignKey(o => o.ShippingAddressId);

        builder.ToTable("orders");
    }
}
