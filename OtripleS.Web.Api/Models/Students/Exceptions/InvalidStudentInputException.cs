// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class InvalidStudentException : Xeption
    {
        public InvalidStudentException(string parameterName, object parameterValue)
            : base(message: $"Invalid student, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }

        public InvalidStudentException()
            : base(message: "Invalid student. Please fix the errors and try again.") { }
    }
}
