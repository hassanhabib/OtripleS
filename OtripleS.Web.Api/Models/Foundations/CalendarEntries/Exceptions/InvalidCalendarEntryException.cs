// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.Foundations.CalendarEntries.Exceptions
{
    public class InvalidCalendarEntryException : Exception
    {
        public InvalidCalendarEntryException(string parameterName, object parameterValue)
            : base($"Invalid calendarEntry, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
