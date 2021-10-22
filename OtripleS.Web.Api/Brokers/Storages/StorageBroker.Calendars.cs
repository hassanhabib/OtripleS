// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Calendars;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Calendar> Calendars { get; set; }

        public async ValueTask<Calendar> InsertCalendarAsync(Calendar calendar)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Calendar> calendarEntityEntry =
                await broker.Calendars.AddAsync(calendar);

            await broker.SaveChangesAsync();

            return calendarEntityEntry.Entity;
        }

        public IQueryable<Calendar> SelectAllCalendars() => this.Calendars.AsQueryable();

        public async ValueTask<Calendar> SelectCalendarByIdAsync(Guid calendarId)
        {
            using var broker = new StorageBroker(this.configuration);

            broker.ChangeTracker.QueryTrackingBehavior =
                QueryTrackingBehavior.NoTracking;

            return await Calendars.FindAsync(calendarId);
        }

        public async ValueTask<Calendar> UpdateCalendarAsync(Calendar calendar)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Calendar> calendarEntityEntry =
                broker.Calendars.Update(calendar);

            await broker.SaveChangesAsync();

            return calendarEntityEntry.Entity;
        }

        public async ValueTask<Calendar> DeleteCalendarAsync(Calendar calendar)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Calendar> calendarEntityEntry =
                broker.Calendars.Remove(calendar);

            await broker.SaveChangesAsync();

            return calendarEntityEntry.Entity;
        }
    }
}
