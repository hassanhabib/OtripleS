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

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Calendar> Calendars { get; set; }

        public async ValueTask<Calendar> InsertCalendarAsync(Calendar calendar)
        {
            EntityEntry<Calendar> calendarEntityEntry = 
                await this.Calendars.AddAsync(calendar);
            
            await this.SaveChangesAsync();

            return calendarEntityEntry.Entity;
        }

        public IQueryable<Calendar> SelectAllCalendars() => this.Calendars.AsQueryable();

        public async ValueTask<Calendar> SelectCalendarByIdAsync(Guid calendarId)
        {
            this.ChangeTracker.QueryTrackingBehavior = 
                QueryTrackingBehavior.NoTracking;

            return await Calendars.FindAsync(calendarId);
        }

        public async ValueTask<Calendar> UpdateCalendarAsync(Calendar calendar)
        {
            EntityEntry<Calendar> calendarEntityEntry = 
                this.Calendars.Update(calendar);
            
            await this.SaveChangesAsync();

            return calendarEntityEntry.Entity;
        }

        public async ValueTask<Calendar> DeleteCalendarAsync(Calendar calendar)
        {
            EntityEntry<Calendar> calendarEntityEntry = 
                this.Calendars.Remove(calendar);
            
            await this.SaveChangesAsync();

            return calendarEntityEntry.Entity;
        }
    }
}
