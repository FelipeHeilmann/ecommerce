using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class OrderTotalAndCouponMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_CouponModel_CouponId1",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_CouponModel_coupon_id",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CouponModel",
                table: "CouponModel");

            migrationBuilder.RenameTable(
                name: "CouponModel",
                newName: "coupons");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "coupons",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "coupons",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "coupons",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ExpiressAt",
                table: "coupons",
                newName: "expiress_at");

            migrationBuilder.AddColumn<double>(
                name: "total",
                table: "orders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_coupons",
                table: "coupons",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_coupons_CouponId1",
                table: "orders",
                column: "CouponId1",
                principalTable: "coupons",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_coupons_coupon_id",
                table: "orders",
                column: "coupon_id",
                principalTable: "coupons",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_coupons_CouponId1",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_coupons_coupon_id",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_coupons",
                table: "coupons");

            migrationBuilder.DropColumn(
                name: "total",
                table: "orders");

            migrationBuilder.RenameTable(
                name: "coupons",
                newName: "CouponModel");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "CouponModel",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "CouponModel",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CouponModel",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "expiress_at",
                table: "CouponModel",
                newName: "ExpiressAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CouponModel",
                table: "CouponModel",
                column: "Id");

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
    }
}
