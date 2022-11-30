// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<CalendarEntryAttachment> CalendarEntriesAttachments { get; set; }

        public async ValueTask<CalendarEntryAttachment> InsertCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<CalendarEntryAttachment> calendarEntryAttachmentEntityEntry =
                await broker.CalendarEntriesAttachments.AddAsync(entity: calendarEntryAttachment);

            await broker.SaveChangesAsync();

            return calendarEntryAttachmentEntityEntry.Entity;
        }

        public IQueryable<CalendarEntryAttachment> SelectAllCalendarEntryAttachment() => SelectAllCalendarEntryAttachment();

        public async ValueTask<CalendarEntryAttachment> SelectCalendarEntryAttachmentByIdAsync(
            Guid calendarEntryId,
            Guid attachmentId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.CalendarEntriesAttachments.FindAsync(calendarEntryId, attachmentId);
        }

        public async ValueTask<CalendarEntryAttachment> UpdateCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<CalendarEntryAttachment> calendarEntryAttachmentEntityEntry =
                broker.CalendarEntriesAttachments.Update(entity: calendarEntryAttachment);

            await broker.SaveChangesAsync();

            return calendarEntryAttachmentEntityEntry.Entity;
        }

        public async ValueTask<CalendarEntryAttachment> DeleteCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<CalendarEntryAttachment> calendarEntryAttachmentEntityEntry =
                broker.CalendarEntriesAttachments.Remove(entity: calendarEntryAttachment);

            await broker.SaveChangesAsync();

            return calendarEntryAttachmentEntityEntry.Entity;
        }
    }
}
