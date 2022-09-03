using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AioCore.Web.Migrations.Settings
{
    public partial class CreateTenantDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantDomains",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantDomains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantDomains_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "Settings",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantDomains_TenantId",
                schema: "Settings",
                table: "TenantDomains",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantDomains",
                schema: "Settings");
        }
    }
}
