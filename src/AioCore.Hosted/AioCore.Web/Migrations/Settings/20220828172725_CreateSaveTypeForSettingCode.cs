using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AioCore.Web.Migrations.Settings
{
    public partial class CreateSaveTypeForSettingCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InlineCode",
                schema: "Settings",
                table: "Codes");

            migrationBuilder.AddColumn<int>(
                name: "SaveType",
                schema: "Settings",
                table: "Codes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaveType",
                schema: "Settings",
                table: "Codes");

            migrationBuilder.AddColumn<bool>(
                name: "InlineCode",
                schema: "Settings",
                table: "Codes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
