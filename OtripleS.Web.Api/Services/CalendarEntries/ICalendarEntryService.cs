// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using OtripleS.Web.Api.Models.CalendarEntries;

namespace OtripleS.Web.Api.Services.CalendarEntries
{
    public interface ICalendarEntryService
    {
        IQueryable<CalendarEntry> RetrieveAllCalendarEntries();
    }
}
