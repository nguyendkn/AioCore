using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AioCore.Web.Migrations.Settings
{
    public partial class UpdateLengthAndIndexForSettingAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attributes_EntityId",
                schema: "Settings",
                table: "Attributes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Settings",
                table: "Attributes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_EntityId_Name",
                schema: "Settings",
                table: "Attributes",
                columns: new[] { "EntityId", "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attributes_EntityId_Name",
                schema: "Settings",
                table: "Attributes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Settings",
                table: "Attributes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_EntityId",
                schema: "Settings",
                table: "Attributes",
                column: "EntityId");
        }
    }
}
