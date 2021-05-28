// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class InvalidStudentRegistrationInputException : Exception
    {
        public InvalidStudentRegistrationInputException(string parameterName, object parameterValue)
            : base($"Invalid StudentRegistration, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
