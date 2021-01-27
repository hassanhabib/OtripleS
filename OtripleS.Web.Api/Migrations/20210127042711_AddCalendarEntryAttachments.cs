// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OtripleS.Web.Api.Migrations
{
    public partial class AddCalendarEntryAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarEntryAttachments",
                columns: table => new
                {
                    CalendarEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEntryAttachments", x => new { x.CalendarEntryId, x.AttachmentId });
                    table.ForeignKey(
                        name: "FK_CalendarEntryAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CalendarEntryAttachments_CalendarEntries_CalendarEntryId",
                        column: x => x.CalendarEntryId,
                        principalTable: "CalendarEntries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntryAttachments_AttachmentId",
                table: "CalendarEntryAttachments",
                column: "AttachmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarEntryAttachments");
        }
    }
}
