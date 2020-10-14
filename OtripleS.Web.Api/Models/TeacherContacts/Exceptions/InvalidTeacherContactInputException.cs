// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.TeacherContacts.Exceptions
{
    public class InvalidTeacherContactInputException : Exception
    {
        public InvalidTeacherContactInputException(string parameterName, object parameterValue)
            : base($"Invalid TeacherContact, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
