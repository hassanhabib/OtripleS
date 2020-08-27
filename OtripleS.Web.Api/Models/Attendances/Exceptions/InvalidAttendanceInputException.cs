// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Attendances.Exceptions
{
    public class InvalidAttendanceInputException : Exception
    {
        public InvalidAttendanceInputException(string parameterName, object parameterValue)
            : base($"Invalid Attendace, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
