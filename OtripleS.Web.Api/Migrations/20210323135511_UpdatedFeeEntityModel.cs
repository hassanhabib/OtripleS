// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OtripleS.Web.Api.Migrations
{
    public partial class UpdatedFeeEntityModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fees_AspNetUsers_UpdatedByUserId",
                table: "Fees");

            migrationBuilder.DropIndex(
                name: "IX_Fees_UpdatedByUserId",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Fees");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_UpdatedBy",
                table: "Fees",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Fees_AspNetUsers_UpdatedBy",
                table: "Fees",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fees_AspNetUsers_UpdatedBy",
                table: "Fees");

            migrationBuilder.DropIndex(
                name: "IX_Fees_UpdatedBy",
                table: "Fees");

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByUserId",
                table: "Fees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fees_UpdatedByUserId",
                table: "Fees",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fees_AspNetUsers_UpdatedByUserId",
                table: "Fees",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
