using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OtripleS.Web.Api.Migrations
{
    public partial class AddStudentExamFees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentExamFees",
                table: "StudentExamFees");

            migrationBuilder.DropIndex(
                name: "IX_StudentExamFees_StudentId",
                table: "StudentExamFees");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentExamFees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentExamFees",
                table: "StudentExamFees",
                columns: new[] { "StudentId", "ExamFeeId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentExamFees",
                table: "StudentExamFees");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "StudentExamFees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentExamFees",
                table: "StudentExamFees",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamFees_StudentId",
                table: "StudentExamFees",
                column: "StudentId");
        }
    }
}
