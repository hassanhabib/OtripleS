// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.CalendarEntries.Exceptions
{
    public class InvalidCalendarEntryException : Xeption
    {
        public InvalidCalendarEntryException(string parameterName, object parameterValue)
            : base($"Invalid calendar entry, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }

        public InvalidCalendarEntryException()
            : base("Invalid calendar enty. Please fix the errors and try again.") { }
    }
}
