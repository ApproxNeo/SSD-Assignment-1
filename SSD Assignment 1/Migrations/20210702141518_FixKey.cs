using Microsoft.EntityFrameworkCore.Migrations;

namespace SSD_Assignment_1.Migrations
{
    public partial class FixKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Product",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Product",
                newName: "ID");
        }
    }
}
