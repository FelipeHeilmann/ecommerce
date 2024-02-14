using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class Address_Table_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_address_billing_address_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_address_shipping_address_id",
                table: "orders");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    zip_code = table.Column<string>(type: "text", nullable: false),
                    street = table.Column<string>(type: "text", nullable: false),
                    neighborhood = table.Column<string>(name: "'neighborhood", type: "text", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    apartament = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_addresses_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_customer_id",
                table: "addresses",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_addresses_billing_address_id",
                table: "orders",
                column: "billing_address_id",
                principalTable: "addresses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_addresses_shipping_address_id",
                table: "orders",
                column: "shipping_address_id",
                principalTable: "addresses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_addresses_billing_address_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_addresses_shipping_address_id",
                table: "orders");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    apartament = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    neighborhood = table.Column<string>(name: "'neighborhood", type: "text", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    street = table.Column<string>(type: "text", nullable: false),
                    zip_code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_address_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_customer_id",
                table: "address",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_address_billing_address_id",
                table: "orders",
                column: "billing_address_id",
                principalTable: "address",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_address_shipping_address_id",
                table: "orders",
                column: "shipping_address_id",
                principalTable: "address",
                principalColumn: "id");
        }
    }
}
