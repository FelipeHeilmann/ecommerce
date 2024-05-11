using Domain.Customers.Entity;
using Domain.Customers.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infra.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {

        var nameConverter = new ValueConverter<Name, string>(name => name.Value, name => new Name(name));
        var emailConverter = new ValueConverter<Email, string>(email => email.Value, email => new Email(email));
        var cpfConverter = new ValueConverter<CPF, string>(cpf => cpf.Value, cpf => new CPF(cpf));
        var phoneConverter = new ValueConverter<Phone, string>(phone => phone.Value, phone => new Phone(phone));

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(typeof(Name), "_name")
            .HasConversion(nameConverter)
            .HasMaxLength(150)
            .HasColumnName("name");
        builder.Property(typeof(Email), "_email")
             .HasConversion(emailConverter)
            .HasMaxLength(250)
            .HasColumnName("email");
        builder.Property(typeof(CPF), "_cpf")
            .HasConversion(cpfConverter)
            .HasMaxLength(11)
            .HasColumnName("cpf");
        builder.Property(typeof(Phone), "_phone")
            .HasConversion(phoneConverter)
            .HasMaxLength(11)
            .HasColumnName("phone");
        builder.Property(c => c.Password).HasColumnName("password");
        builder.Property(c => c.BirthDate).HasColumnName("birth_date").HasColumnType("date");
        builder.Property(c => c.CreatedAt).HasColumnName("created_at");

        builder.ToTable("customers");
    }
}