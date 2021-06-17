// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.CalendarEntries;

namespace OtripleS.Web.Api.Services.Foundations.CalendarEntries
{
    public interface ICalendarEntryService
    {
        ValueTask<CalendarEntry> AddCalendarEntryAsync(CalendarEntry calendarEntry);
        IQueryable<CalendarEntry> RetrieveAllCalendarEntries();
        ValueTask<CalendarEntry> RetrieveCalendarEntryByIdAsync(Guid calendarEntryId);
        ValueTask<CalendarEntry> ModifyCalendarEntryAsync(CalendarEntry calendarEntry);
        ValueTask<CalendarEntry> RemoveCalendarEntryByIdAsync(Guid calendarEntryId);
    }
}
