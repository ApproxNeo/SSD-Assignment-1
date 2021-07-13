using Microsoft.EntityFrameworkCore.Migrations;

namespace SSD_Assignment_1.Migrations
{
    public partial class UpdateCheckout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Order",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IntentId",
                table: "Order",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Order",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Order",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IntentId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Order");
        }
    }
}
