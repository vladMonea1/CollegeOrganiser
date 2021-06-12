using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeOrganiser.Data.Migrations
{
    public partial class AttendancesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(nullable: true),
                    CourseOwner = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoursesHeld",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateHeld = table.Column<DateTime>(nullable: false),
                    CourseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesHeld", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursesHeld_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCourseHeld = table.Column<int>(nullable: false),
                    IdStudent = table.Column<int>(nullable: false),
                    Attended = table.Column<bool>(nullable: false),
                    CourseAttendedId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAttendances_CoursesHeld_CourseAttendedId",
                        column: x => x.CourseAttendedId,
                        principalTable: "CoursesHeld",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseAttendances_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseAttendances_CourseAttendedId",
                table: "CourseAttendances",
                column: "CourseAttendedId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAttendances_UserId",
                table: "CourseAttendances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesHeld_CourseId",
                table: "CoursesHeld",
                column: "CourseId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseAttendances");

            migrationBuilder.DropTable(
                name: "CoursesHeld");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
