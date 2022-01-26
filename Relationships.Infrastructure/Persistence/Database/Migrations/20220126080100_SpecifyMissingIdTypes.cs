using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Relationships.Infrastructure.Persistence.Database.Migrations
{
    public partial class SpecifyMissingIdTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedByDevice",
                table: "RelationshipTemplates",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "RelationshipTemplates",
                type: "char(36)",
                unicode: false,
                fixedLength: true,
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)");

            migrationBuilder.AlterColumn<string>(
                name: "Res_CreatedByDevice",
                table: "RelationshipChanges",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Res_CreatedBy",
                table: "RelationshipChanges",
                type: "char(36)",
                unicode: false,
                fixedLength: true,
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Req_CreatedByDevice",
                table: "RelationshipChanges",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "Req_CreatedBy",
                table: "RelationshipChanges",
                type: "char(36)",
                unicode: false,
                fixedLength: true,
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "RelationshipChanges",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedByDevice",
                table: "RelationshipTemplates",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "RelationshipTemplates",
                type: "nvarchar(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "Res_CreatedByDevice",
                table: "RelationshipChanges",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Res_CreatedBy",
                table: "RelationshipChanges",
                type: "nvarchar(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Req_CreatedByDevice",
                table: "RelationshipChanges",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Req_CreatedBy",
                table: "RelationshipChanges",
                type: "nvarchar(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "RelationshipChanges",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20);
        }
    }
}
