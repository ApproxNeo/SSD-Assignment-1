using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSD_Assignment_1.Migrations
{
    public partial class AddAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleAuditRecord",
                columns: table => new
                {
                    Audit_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditActionType = table.Column<string>(nullable: true),
                    PerformedBy = table.Column<string>(nullable: true),
                    PerformedOn = table.Column<string>(nullable: true),
                    DateTimeStamp = table.Column<DateTime>(nullable: false),
                    KeyAuditFieldID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAuditRecord", x => x.Audit_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleAuditRecord");
        }
    }
}
