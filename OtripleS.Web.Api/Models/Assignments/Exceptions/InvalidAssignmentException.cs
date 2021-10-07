// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Assignments.Exceptions
{
    public class InvalidAssignmentException : Xeption
    {
        public InvalidAssignmentException(string parameterName, object parameterValue)
            : base(message: $"Invalid assignment, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }

        public InvalidAssignmentException()
            : base(message: "Invalid assignment. Please fix the errors and try again.") { }
    }
}
