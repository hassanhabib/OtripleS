// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Calendars.Exceptions
{
    public class AlreadyExistsCalendarException : Exception
    {
        public AlreadyExistsCalendarException(Exception innerException)
            : base("Calendar with the same id already exists.", innerException) { }
    }
}
