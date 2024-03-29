﻿using Domain.Refunds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.TransactionId).HasColumnName("transction_id");
        builder.Property(r => r.Amount).HasColumnName("amount");
        builder.Property(r => r.Status).HasColumnName("status");
        builder.Property(r => r.CreatedAt).HasColumnName("created_at");
        builder.Property(r => r.PayedAt).HasColumnName("payed_at");

        builder.ToTable("refunds");
    }
}
