using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeOrganiser.Data.Migrations
{
    public partial class CoursesForUserMigration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCourseHeld",
                table: "CourseAttendances");

            migrationBuilder.DropColumn(
                name: "IdStudent",
                table: "CourseAttendances");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCourseHeld",
                table: "CourseAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdStudent",
                table: "CourseAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
