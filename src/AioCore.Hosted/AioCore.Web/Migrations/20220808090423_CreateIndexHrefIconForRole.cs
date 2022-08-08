using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AioCore.Web.Migrations
{
    public partial class CreateIndexHrefIconForRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                schema: "Identity",
                table: "AspNetRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_ParentId",
                schema: "Identity",
                table: "AspNetRoles",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetRoles_ParentId",
                schema: "Identity",
                table: "AspNetRoles",
                column: "ParentId",
                principalSchema: "Identity",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetRoles_ParentId",
                schema: "Identity",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_ParentId",
                schema: "Identity",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "Identity",
                table: "AspNetRoles");
        }
    }
}
