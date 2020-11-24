using System;
namespace OtripleS.Web.Api.Models.Calendars.Exceptions
{
    public class NotFoundCalendarException : Exception
    {
        public NotFoundCalendarException(Guid calendarId)
            : base($"Couldn't find calendar with Id: {calendarId}.") { }
    }
}
