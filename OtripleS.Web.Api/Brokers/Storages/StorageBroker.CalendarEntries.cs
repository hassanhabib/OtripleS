// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Foundations.CalendarEntries;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<CalendarEntry> CalendarEntries { get; set; }

        public async ValueTask<CalendarEntry> InsertCalendarEntryAsync(CalendarEntry calendarEntry)
        {
            EntityEntry<CalendarEntry> calendarEntryEntityEntry =
                await this.CalendarEntries.AddAsync(calendarEntry);

            await this.SaveChangesAsync();

            return calendarEntryEntityEntry.Entity;
        }

        public IQueryable<CalendarEntry> SelectAllCalendarEntries() => this.CalendarEntries.AsQueryable();

        public async ValueTask<CalendarEntry> SelectCalendarEntryByIdAsync(Guid calendarEntryId)
        {
            this.ChangeTracker.QueryTrackingBehavior =
                QueryTrackingBehavior.NoTracking;

            return await CalendarEntries.FindAsync(calendarEntryId);
        }

        public async ValueTask<CalendarEntry> UpdateCalendarEntryAsync(CalendarEntry calendarEntry)
        {
            EntityEntry<CalendarEntry> calendarEntryEntityEntry =
                this.CalendarEntries.Update(calendarEntry);

            await this.SaveChangesAsync();

            return calendarEntryEntityEntry.Entity;
        }

        public async ValueTask<CalendarEntry> DeleteCalendarEntryAsync(CalendarEntry calendarEntry)
        {
            EntityEntry<CalendarEntry> calendarEntryEntityEntry =
                this.CalendarEntries.Remove(calendarEntry);

            await this.SaveChangesAsync();

            return calendarEntryEntityEntry.Entity;
        }
    }
}
