// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Linq;
using OtripleS.Web.Api.Models.CalendarEntries;
using System;

namespace OtripleS.Web.Api.Services.CalendarEntries
{
    public interface ICalendarEntryService
    {
        ValueTask<CalendarEntry> AddCalendarEntryAsync(CalendarEntry calendarEntry);
        IQueryable<CalendarEntry> RetrieveAllCalendarEntries();
        ValueTask<CalendarEntry> RetrieveCalendarEntryByIdAsync(Guid calendarEntryId);
        ValueTask<CalendarEntry> DeleteCalendarEntryByIdAsync(Guid calendarEntryId);
    }
}
