using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeOrganiser.Data.Migrations
{
    public partial class CoursesForUserMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoursesForUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedToCourse = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CoursesAssignedId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesForUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursesForUsers_Courses_CoursesAssignedId",
                        column: x => x.CoursesAssignedId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoursesForUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoursesForUsers_CoursesAssignedId",
                table: "CoursesForUsers",
                column: "CoursesAssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesForUsers_UserId",
                table: "CoursesForUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoursesForUsers");
        }
    }
}
