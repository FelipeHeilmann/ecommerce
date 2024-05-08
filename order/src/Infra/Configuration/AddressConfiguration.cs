using Domain.Addresses.Entity;
using Domain.Customers.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.CustomerId).HasColumnName("customer_id");
        builder.Property(a => a.ZipCode).HasColumnName("zip_code");
        builder.Property(a => a.Street).HasColumnName("street");
        builder.Property(a => a.Neighborhood).HasColumnName("'neighborhood");
        builder.Property(a => a.Number).HasColumnName("number");
        builder.Property(a => a.State).HasColumnName("state");
        builder.Property(a => a.Complement).HasColumnName("complement");
        builder.Property(a => a.City).HasColumnName("city");
        builder.Property(a => a.Country).HasColumnName("country");

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(a => a.CustomerId);

        builder.ToTable("addresses");
            
    }
}
