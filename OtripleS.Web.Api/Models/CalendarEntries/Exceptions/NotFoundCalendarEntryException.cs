using System;

namespace OtripleS.Web.Api.Models.CalendarEntries.Exceptions
{
    public class NotFoundCalendarEntryException : Exception
    {
        public NotFoundCalendarEntryException(Guid calendarEntryId)
            : base($"Couldn't find calendar entry with Id: {calendarEntryId}.") { }
    }
}
