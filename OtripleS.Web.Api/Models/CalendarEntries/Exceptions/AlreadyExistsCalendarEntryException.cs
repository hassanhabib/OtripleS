// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CalendarEntries.Exceptions
{
    public class AlreadyExistsCalendarEntryException : Exception
    {
        public AlreadyExistsCalendarEntryException(Exception innerException)
            : base(message: "Calendar entry with the same id already exists.", innerException) { }
    }
}
