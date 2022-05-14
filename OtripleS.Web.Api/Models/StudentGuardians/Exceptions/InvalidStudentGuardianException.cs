// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentGuardians.Exceptions
{
    public class InvalidStudentGuardiantException : Exception
    { 
        public InvalidStudentGuardiantException(string parameterName, object parameterValue)
            : base(message: $"Invalid student guardian, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }
    }
}
