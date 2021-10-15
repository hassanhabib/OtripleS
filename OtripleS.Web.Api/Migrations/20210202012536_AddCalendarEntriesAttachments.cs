// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace OtripleS.Web.Api.Migrations
{
    public partial class AddCalendarEntriesAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarEntriesAttachments",
                columns: table => new
                {
                    CalendarEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEntriesAttachments", x => new { x.CalendarEntryId, x.AttachmentId });
                    table.ForeignKey(
                        name: "FK_CalendarEntriesAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CalendarEntriesAttachments_CalendarEntries_CalendarEntryId",
                        column: x => x.CalendarEntryId,
                        principalTable: "CalendarEntries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntriesAttachments_AttachmentId",
                table: "CalendarEntriesAttachments",
                column: "AttachmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarEntriesAttachments");
        }
    }
}
