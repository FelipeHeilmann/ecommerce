using Domain.Customers;
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
            .HasConversion(name => name.Value, value => Name.Create(value).Value)
            .HasMaxLength(150)
            .HasColumnName("name");
        builder.Property(c => c.Email)
            .HasConversion(email => email.Value, value => Email.Create(value).Value)
            .HasMaxLength(250)
            .HasColumnName("email");
        builder.Property(c => c.CPF)
            .HasConversion(cpf => cpf.Value, value => CPF.Create(value).Value)
            .HasMaxLength(11)
            .HasColumnName("cpf");
        builder.HasIndex(c => c.Email);
        builder.Property(c => c.Password).HasColumnName("password");
        builder.Property(c => c.BirthDate).HasColumnName("birth_date").HasColumnType("date");
        builder.Property(c => c.CreatedAt).HasColumnName("created_at");

        builder.ToTable("customers");
    }
}
