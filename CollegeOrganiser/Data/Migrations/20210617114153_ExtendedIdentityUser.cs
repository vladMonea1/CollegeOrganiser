using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeOrganiser.Data.Migrations
{
    public partial class ExtendedIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeUtilizator",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeUtilizator",
                table: "AspNetUsers");
        }
    }
}
