// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.CalendarEntries;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<CalendarEntry> CalendarEntries { get; set; }

        public async ValueTask<CalendarEntry> InsertCalendarEntryAsync(CalendarEntry calendarEntry)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<CalendarEntry> calendarEntryEntityEntry =
                await broker.CalendarEntries.AddAsync(entity: calendarEntry);

            await broker.SaveChangesAsync();

            return calendarEntryEntityEntry.Entity;
        }

        public IQueryable<CalendarEntry> SelectAllCalendarEntries() => this.CalendarEntries;

        public async ValueTask<CalendarEntry> SelectCalendarEntryByIdAsync(Guid calendarEntryId)
        {
            using var broker = new StorageBroker(this.configuration);

            broker.ChangeTracker.QueryTrackingBehavior =
                QueryTrackingBehavior.NoTracking;

            return await broker.CalendarEntries.FindAsync(calendarEntryId);
        }

        public async ValueTask<CalendarEntry> UpdateCalendarEntryAsync(CalendarEntry calendarEntry)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<CalendarEntry> calendarEntryEntityEntry =
                broker.CalendarEntries.Update(entity: calendarEntry);

            await broker.SaveChangesAsync();

            return calendarEntryEntityEntry.Entity;
        }

        public async ValueTask<CalendarEntry> DeleteCalendarEntryAsync(CalendarEntry calendarEntry)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<CalendarEntry> calendarEntryEntityEntry =
                broker.CalendarEntries.Remove(entity: calendarEntry);

            await broker.SaveChangesAsync();

            return calendarEntryEntityEntry.Entity;
        }
    }
}
