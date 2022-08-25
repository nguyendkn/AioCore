using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AioCore.Web.Migrations.Settings
{
    public partial class CreateSettingCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Codes",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PathFile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Codes_Codes_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Settings",
                        principalTable: "Codes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Codes_ParentId",
                schema: "Settings",
                table: "Codes",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Codes",
                schema: "Settings");
        }
    }
}
