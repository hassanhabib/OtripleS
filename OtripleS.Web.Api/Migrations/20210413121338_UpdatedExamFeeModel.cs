// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

namespace OtripleS.Web.Api.Migrations
{
    public partial class UpdatedExamFeeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamFees_AspNetUsers_FeeId",
                table: "ExamFees");

            migrationBuilder.DropIndex(
                name: "IX_ExamFees_FeeId",
                table: "ExamFees");

            migrationBuilder.CreateIndex(
                name: "IX_ExamFees_UpdatedBy",
                table: "ExamFees",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamFees_AspNetUsers_UpdatedBy",
                table: "ExamFees",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamFees_AspNetUsers_UpdatedBy",
                table: "ExamFees");

            migrationBuilder.DropIndex(
                name: "IX_ExamFees_UpdatedBy",
                table: "ExamFees");

            migrationBuilder.CreateIndex(
                name: "IX_ExamFees_FeeId",
                table: "ExamFees",
                column: "FeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamFees_AspNetUsers_FeeId",
                table: "ExamFees",
                column: "FeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
