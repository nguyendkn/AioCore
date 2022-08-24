using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AioCore.Web.Migrations.Settings
{
    public partial class CreateReferenceEntityAndForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                schema: "Settings",
                table: "Forms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_EntityId",
                schema: "Settings",
                table: "Forms",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Entities_EntityId",
                schema: "Settings",
                table: "Forms",
                column: "EntityId",
                principalSchema: "Settings",
                principalTable: "Entities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Entities_EntityId",
                schema: "Settings",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_EntityId",
                schema: "Settings",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "Settings",
                table: "Forms");
        }
    }
}
