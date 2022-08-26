using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AioCore.Web.Migrations.Settings
{
    public partial class RemovePathFileSettingCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathFile",
                schema: "Settings",
                table: "Codes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathFile",
                schema: "Settings",
                table: "Codes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
