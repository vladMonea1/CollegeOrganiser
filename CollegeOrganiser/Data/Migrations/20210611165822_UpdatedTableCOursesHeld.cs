using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeOrganiser.Data.Migrations
{
    public partial class UpdatedTableCOursesHeld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CoursesHeld_CourseId",
                table: "CoursesHeld");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CoursesHeld",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesHeld_CourseId",
                table: "CoursesHeld",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CoursesHeld_CourseId",
                table: "CoursesHeld");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CoursesHeld",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoursesHeld_CourseId",
                table: "CoursesHeld",
                column: "CourseId",
                unique: true);
        }
    }
}
