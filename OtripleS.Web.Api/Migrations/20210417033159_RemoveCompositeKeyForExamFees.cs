// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

namespace OtripleS.Web.Api.Migrations
{
    public partial class RemoveCompositeKeyForExamFees : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_ExamFees_ExamId",
                table: "ExamFees",
                column: "ExamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
