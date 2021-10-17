// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace OtripleS.Web.Api.Migrations
{
    public partial class AddStudentExamFees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamFees",
                table: "ExamFees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamFees",
                table: "ExamFees",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StudentExamFees",
                columns: table => new
                {
                    ExamFeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExamFees", x => new { x.StudentId, x.ExamFeeId });
                    table.ForeignKey(
                        name: "FK_StudentExamFees_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentExamFees_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentExamFees_ExamFees_ExamFeeId",
                        column: x => x.ExamFeeId,
                        principalTable: "ExamFees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentExamFees_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamFees_ExamId",
                table: "ExamFees",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamFees_CreatedBy",
                table: "StudentExamFees",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamFees_ExamFeeId",
                table: "StudentExamFees",
                column: "ExamFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamFees_UpdatedBy",
                table: "StudentExamFees",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentExamFees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamFees",
                table: "ExamFees");

            migrationBuilder.DropIndex(
                name: "IX_ExamFees_ExamId",
                table: "ExamFees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamFees",
                table: "ExamFees",
                columns: new[] { "ExamId", "FeeId" });
        }
    }
}
