using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderCouponForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_coupons_CouponId1",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_CouponId1",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CouponId1",
                table: "orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CouponId1",
                table: "orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_CouponId1",
                table: "orders",
                column: "CouponId1");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_coupons_CouponId1",
                table: "orders",
                column: "CouponId1",
                principalTable: "coupons",
                principalColumn: "id");
        }
    }
}
