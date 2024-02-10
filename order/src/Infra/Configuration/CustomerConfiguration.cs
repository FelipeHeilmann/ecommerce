using Domain.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.Name)
            .HasConversion(name => name.Value, value => Name.Create(value))
            .HasMaxLength(150)
            .HasColumnName("name");
        builder.Property(c => c.Email)
            .HasConversion(email => email.Value, value => Email.Create(value))
            .HasMaxLength(250)
            .HasColumnName("email");    
        builder.HasIndex(c => c.Email.Value);
        builder.Property(c => c.Password).HasColumnName("password");
        builder.Property(c => c.BirhDate).HasColumnName("birth_date");
        builder.Property(c => c.CreatedAt).HasColumnName("created_at");

        builder.ToTable("customers");
    }
}
