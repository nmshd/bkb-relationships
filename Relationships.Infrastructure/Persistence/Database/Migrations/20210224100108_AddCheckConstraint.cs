using Microsoft.EntityFrameworkCore.Migrations;

namespace Relationships.Infrastructure.Persistence.Database.Migrations
{
    public partial class AddCheckConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteCheckConstraint();
        }
    }
}
