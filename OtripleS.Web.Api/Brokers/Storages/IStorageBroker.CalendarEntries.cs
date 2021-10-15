// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.CalendarEntries;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<CalendarEntry> InsertCalendarEntryAsync(CalendarEntry calendarEntry);
        IQueryable<CalendarEntry> SelectAllCalendarEntries();
        ValueTask<CalendarEntry> SelectCalendarEntryByIdAsync(Guid calendarEntryId);
        ValueTask<CalendarEntry> UpdateCalendarEntryAsync(CalendarEntry calendarEntry);
        ValueTask<CalendarEntry> DeleteCalendarEntryAsync(CalendarEntry calendarEntry);
    }
}
