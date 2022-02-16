// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetCalendarEntryAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalendarEntryAttachment>()
                .HasKey(calendarEntryAttachment =>
                    new { calendarEntryAttachment.CalendarEntryId, calendarEntryAttachment.AttachmentId });

            modelBuilder.Entity<CalendarEntryAttachment>()
                .HasOne(calendarEntryAttachment => calendarEntryAttachment.Attachment)
                .WithMany(attachment => attachment.CalendarEntryAttachments)
                .HasForeignKey(calendarEntryAttachment => calendarEntryAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CalendarEntryAttachment>()
                .HasOne(calendarEntryAttachment => calendarEntryAttachment.CalendarEntry)
                .WithMany(calendarEntry => calendarEntry.CalendarEntryAttachments)
                .HasForeignKey(calendarEntryAttachment => calendarEntryAttachment.CalendarEntryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
