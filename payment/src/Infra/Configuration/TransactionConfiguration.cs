using Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infra.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder) 
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id");
        builder.Property(t => t.OrderId).HasColumnName("order_id");
        builder.Property(t => t.Amount).HasColumnName("amount");
        builder.Property(t => t.PaymentServiceId).HasColumnName("payment_service_id");
        builder.Property(t => t.PaymentType).HasColumnName("payment_type");
        builder.Property(t => t.Status).HasColumnName("status");
        builder.Property(t => t.CustomerId).HasColumnName("customer_id");
        builder.Property(t => t.CreatedAt).HasColumnName("created_at");
        builder.Property(t => t.ApprovedAt).HasColumnName("approved_at");
        builder.Property(t => t.RefusedAt).HasColumnName("refused_at");

        builder.ToTable("transactions");
    }
}
