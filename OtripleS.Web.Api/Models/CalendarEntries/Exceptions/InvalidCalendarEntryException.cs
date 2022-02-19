// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.CalendarEntries.Exceptions
{
    public class InvalidCalendarEntryException : Xeption
    {
        public InvalidCalendarEntryException(string parameterName, object parameterValue)
            : base(message: $"Invalid calendar entry, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }

        public InvalidCalendarEntryException()
            : base(message: "Invalid calendar entry. Please fix the errors and try again.") { }
    }
}
