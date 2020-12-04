using System;

namespace OtripleS.Web.Api.Models.CalendarEntries.Exceptions
{
    public class LockedCalendarEntryException : Exception
    {
        public LockedCalendarEntryException(Exception innerException)
            : base("Locked calendar entry record exception, please try again later.", innerException) { }
    }
}
