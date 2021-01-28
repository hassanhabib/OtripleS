// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<CalendarEntryAttachment> CalendarEntryAttachments { get; set; }

        public async ValueTask<CalendarEntryAttachment> InsertCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment)
        {
            EntityEntry<CalendarEntryAttachment> calendarEntryAttachmentEntityEntry =
                await this.CalendarEntryAttachments.AddAsync(calendarEntryAttachment);

            await this.SaveChangesAsync();

            return calendarEntryAttachmentEntityEntry.Entity;
        }

        public IQueryable<CalendarEntryAttachment> SelectAllCalendarEntryAttachments() =>
            this.CalendarEntryAttachments.AsQueryable();

        public async ValueTask<CalendarEntryAttachment> SelectCalendarEntryAttachmentByIdAsync(
            Guid calendarEntryId,
            Guid attachmentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.CalendarEntryAttachments.FindAsync(calendarEntryId, attachmentId);
        }

        public async ValueTask<CalendarEntryAttachment> UpdateCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment)
        {
            EntityEntry<CalendarEntryAttachment> calendarEntryAttachmentEntityEntry =
                this.CalendarEntryAttachments.Update(calendarEntryAttachment);

            await this.SaveChangesAsync();

            return calendarEntryAttachmentEntityEntry.Entity;
        }

        public async ValueTask<CalendarEntryAttachment> DeleteCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment)
        {
            EntityEntry<CalendarEntryAttachment> calendarEntryAttachmentEntityEntry =
                this.CalendarEntryAttachments.Remove(calendarEntryAttachment);

            await this.SaveChangesAsync();

            return calendarEntryAttachmentEntityEntry.Entity;
        }
    }
}
