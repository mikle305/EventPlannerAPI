using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventPlannerAPI.Migrations
{
    public partial class Test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Test",
                table: "Users",
                type: "int",
                nullable: true);
        }
    }
}
