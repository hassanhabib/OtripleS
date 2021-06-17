// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Foundations.StudentRegistrations.Exceptions
{
    public class InvalidStudentRegistrationException : Exception
    {
        public InvalidStudentRegistrationException(string parameterName, object parameterValue)
            : base($"Invalid StudentRegistration, " +
                  $"ParameterNmae: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}