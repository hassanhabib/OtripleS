// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

namespace OtripleS.Web.Api.Migrations
{
    public partial class AddedExamFeeModelRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamFees",
                table: "ExamFees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamFees",
                table: "ExamFees",
                columns: new[] { "ExamId", "FeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamFees_FeeId",
                table: "ExamFees",
                column: "FeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamFees_Exams_ExamId",
                table: "ExamFees",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamFees_Fees_FeeId",
                table: "ExamFees",
                column: "FeeId",
                principalTable: "Fees",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamFees_Exams_ExamId",
                table: "ExamFees");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamFees_Fees_FeeId",
                table: "ExamFees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamFees",
                table: "ExamFees");

            migrationBuilder.DropIndex(
                name: "IX_ExamFees_FeeId",
                table: "ExamFees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamFees",
                table: "ExamFees",
                column: "Id");
        }
    }
}
