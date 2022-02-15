// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CalendarEntries.Exceptions
{
    public class NotFoundCalendarEntryException : Exception
    {
        public NotFoundCalendarEntryException(Guid calendarEntryId)
            : base(message: $"Couldn't find calendar entry with id: {calendarEntryId}.") { }
    }
}
