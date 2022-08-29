using System;
using AioCore.Domain.DatabaseContexts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIoCore.Migrations.Migrations
{
    public partial class DynamicDatabase : Migration
    {
        public static string Schema = DynamicContext.Schema;

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: Schema);

            migrationBuilder.CreateTable(
                name: "Attributes",
                schema: Schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Attributes", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Entities",
                schema: Schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "xml", nullable: false),
                    EntityTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Entities", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "DateValues",
                schema: Schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DateValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: Schema,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DateValues_Entities_EntityId",
                        column: x => x.EntityId,
                        principalSchema: Schema,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FloatValues",
                schema: Schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloatValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FloatValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: Schema,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FloatValues_Entities_EntityId",
                        column: x => x.EntityId,
                        principalSchema: Schema,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuidValues",
                schema: Schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuidValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuidValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: Schema,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuidValues_Entities_EntityId",
                        column: x => x.EntityId,
                        principalSchema: Schema,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntegerValues",
                schema: Schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegerValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegerValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: Schema,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntegerValues_Entities_EntityId",
                        column: x => x.EntityId,
                        principalSchema: Schema,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StringValues",
                schema: Schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StringValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StringValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: Schema,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StringValues_Entities_EntityId",
                        column: x => x.EntityId,
                        principalSchema: Schema,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DateValues_AttributeId",
                schema: Schema,
                table: "DateValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_DateValues_EntityId",
                schema: Schema,
                table: "DateValues",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FloatValues_AttributeId",
                schema: Schema,
                table: "FloatValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_FloatValues_EntityId",
                schema: Schema,
                table: "FloatValues",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_GuidValues_AttributeId",
                schema: Schema,
                table: "GuidValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_GuidValues_EntityId",
                schema: Schema,
                table: "GuidValues",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegerValues_AttributeId",
                schema: Schema,
                table: "IntegerValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegerValues_EntityId",
                schema: Schema,
                table: "IntegerValues",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_StringValues_AttributeId",
                schema: Schema,
                table: "StringValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_StringValues_EntityId",
                schema: Schema,
                table: "StringValues",
                column: "EntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DateValues",
                schema: Schema);

            migrationBuilder.DropTable(
                name: "FloatValues",
                schema: Schema);

            migrationBuilder.DropTable(
                name: "GuidValues",
                schema: Schema);

            migrationBuilder.DropTable(
                name: "IntegerValues",
                schema: Schema);

            migrationBuilder.DropTable(
                name: "StringValues",
                schema: Schema);

            migrationBuilder.DropTable(
                name: "Attributes",
                schema: Schema);

            migrationBuilder.DropTable(
                name: "Entities",
                schema: Schema);
        }
    }
}