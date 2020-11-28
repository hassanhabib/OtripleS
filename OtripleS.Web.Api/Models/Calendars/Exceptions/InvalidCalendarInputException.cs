// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.Calendars.Exceptions
{
    public class InvalidCalendarInputException : Exception
    {
        public InvalidCalendarInputException(string parameterName, object parameterValue)
            : base($"Invalid Calendar, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        {}
    }
}
