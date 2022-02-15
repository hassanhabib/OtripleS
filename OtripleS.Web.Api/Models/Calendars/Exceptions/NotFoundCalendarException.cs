// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Calendars.Exceptions
{
    public class NotFoundCalendarException : Exception
    {
        public NotFoundCalendarException(Guid calendarId)
            : base(message: $"Couldn't find calendar with id: {calendarId}.") { }
    }
}
