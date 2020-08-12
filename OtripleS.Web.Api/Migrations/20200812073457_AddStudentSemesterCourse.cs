// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OtripleS.Web.Api.Migrations
{
    public partial class AddStudentSemesterCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentSemesterCourses",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(nullable: false),
                    SemesterCourseId = table.Column<Guid>(nullable: false),
                    Grade = table.Column<string>(nullable: true),
                    Score = table.Column<double>(nullable: false),
                    Repeats = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSemesterCourses", x => new { x.StudentId, x.SemesterCourseId });
                    table.ForeignKey(
                        name: "FK_StudentSemesterCourses_SemesterCourses_SemesterCourseId",
                        column: x => x.SemesterCourseId,
                        principalTable: "SemesterCourses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentSemesterCourses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentSemesterCourses_SemesterCourseId",
                table: "StudentSemesterCourses",
                column: "SemesterCourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentSemesterCourses");
        }
    }
}
