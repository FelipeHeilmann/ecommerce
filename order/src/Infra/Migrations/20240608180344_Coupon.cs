using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class Coupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CouponId1",
                table: "orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "coupon_id",
                table: "orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CouponModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    ExpiressAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_CouponId1",
                table: "orders",
                column: "CouponId1");

            migrationBuilder.CreateIndex(
                name: "IX_orders_coupon_id",
                table: "orders",
                column: "coupon_id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_CouponModel_CouponId1",
                table: "orders",
                column: "CouponId1",
                principalTable: "CouponModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_CouponModel_coupon_id",
                table: "orders",
                column: "coupon_id",
                principalTable: "CouponModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_CouponModel_CouponId1",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_CouponModel_coupon_id",
                table: "orders");

            migrationBuilder.DropTable(
                name: "CouponModel");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_orders_CouponId1",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_coupon_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CouponId1",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "coupon_id",
                table: "orders");
        }
    }
}
