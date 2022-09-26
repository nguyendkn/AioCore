using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AioCore.Web.Migrations
{
    public partial class InitialDSettingsatabÃase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Settings");

            migrationBuilder.CreateSequence<int>(
                name: "Sequence_D14134D2B12E52CA5E1278F73D12928F",
                schema: "Settings");

            migrationBuilder.CreateTable(
                name: "Components",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Template = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Href = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Layouts",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PathType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GridColumn = table.Column<int>(type: "int", nullable: false),
                    GridGap = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantGroups",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rows",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LayoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ColumnSpan = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rows_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalSchema: "Settings",
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rows_Layouts_LayoutId",
                        column: x => x.LayoutId,
                        principalSchema: "Settings",
                        principalTable: "Layouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenants_TenantGroups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Settings",
                        principalTable: "TenantGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Codes",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PathType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaveType = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Singled = table.Column<bool>(type: "bit", nullable: false),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Codes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "Settings",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entities",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSource = table.Column<int>(type: "int", nullable: false),
                    SourcePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entities_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "Settings",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [Settings].Sequence_D14134D2B12E52CA5E1278F73D12928F"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AttributeType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attributes_Entities_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "Settings",
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntityCodes",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityCodes_Codes_CodeId",
                        column: x => x.CodeId,
                        principalSchema: "Settings",
                        principalTable: "Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntityCodes_Entities_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "Settings",
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_Entities_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "Settings",
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormAttributes",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ColSpan = table.Column<int>(type: "int", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "Settings",
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormAttributes_Forms_FormId",
                        column: x => x.FormId,
                        principalSchema: "Settings",
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_EntityId_Name",
                schema: "Settings",
                table: "Attributes",
                columns: new[] { "EntityId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_Order",
                schema: "Settings",
                table: "Attributes",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Codes_ParentId",
                schema: "Settings",
                table: "Codes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Codes_TenantId",
                schema: "Settings",
                table: "Codes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_TenantId",
                schema: "Settings",
                table: "Entities",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCodes_CodeId",
                schema: "Settings",
                table: "EntityCodes",
                column: "CodeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCodes_EntityId",
                schema: "Settings",
                table: "EntityCodes",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FormAttributes_AttributeId",
                schema: "Settings",
                table: "FormAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormAttributes_FormId",
                schema: "Settings",
                table: "FormAttributes",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_EntityId",
                schema: "Settings",
                table: "Forms",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Rows_ComponentId",
                schema: "Settings",
                table: "Rows",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Rows_LayoutId",
                schema: "Settings",
                table: "Rows",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantDomains_TenantId",
                schema: "Settings",
                table: "TenantDomains",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_GroupId",
                schema: "Settings",
                table: "Tenants",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityCodes",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Features",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "FormAttributes",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Rows",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "TenantDomains",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Codes",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Attributes",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Forms",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Components",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Layouts",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Entities",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "TenantGroups",
                schema: "Settings");

            migrationBuilder.DropSequence(
                name: "Sequence_D14134D2B12E52CA5E1278F73D12928F",
                schema: "Settings");
        }
    }
}
