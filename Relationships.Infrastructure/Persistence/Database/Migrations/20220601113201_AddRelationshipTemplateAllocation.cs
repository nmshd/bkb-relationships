using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Relationships.Infrastructure.Persistence.Database.Migrations
{
    public partial class AddRelationshipTemplateAllocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxNumberOfAllocations",
                table: "RelationshipTemplates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RelationshipTemplateAllocations",
                columns: table => new
                {
                    RelationshipTemplateId = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: false),
                    AllocatedBy = table.Column<string>(type: "char(36)", unicode: false, fixedLength: true, maxLength: 36, nullable: false),
                    AllocatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AllocatedByDevice = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipTemplateAllocations", x => new { x.RelationshipTemplateId, x.AllocatedBy });
                    table.ForeignKey(
                        name: "FK_RelationshipTemplateAllocations_RelationshipTemplates_RelationshipTemplateId",
                        column: x => x.RelationshipTemplateId,
                        principalTable: "RelationshipTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelationshipTemplateAllocations");

            migrationBuilder.DropColumn(
                name: "MaxNumberOfAllocations",
                table: "RelationshipTemplates");
        }
    }
}
