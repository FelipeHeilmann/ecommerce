﻿// <auto-generated />
using System;
using Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240301220857_Create_Refund_Transaction")]
    partial class Create_Refund_Transaction
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Refunds.Refund", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision")
                        .HasColumnName("amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("PayedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("payed_at");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("transction_id");

                    b.HasKey("Id");

                    b.ToTable("refunds", (string)null);
                });

            modelBuilder.Entity("Domain.Transactions.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision")
                        .HasColumnName("amount");

                    b.Property<DateTime?>("ApprovedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("approved_at");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<Guid>("PaymentServiceId")
                        .HasColumnType("uuid")
                        .HasColumnName("payment_service_id");

                    b.Property<int>("PaymentType")
                        .HasColumnType("integer")
                        .HasColumnName("payment_type");

                    b.Property<DateTime?>("RefusedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refused_at");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.ToTable("transactions", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
