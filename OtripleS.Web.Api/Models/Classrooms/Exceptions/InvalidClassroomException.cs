// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Classrooms.Exceptions
{
    public class InvalidClassroomException : Xeption
    {
        public InvalidClassroomException(string parameterName, object parameterValue)
            : base(message: $"Invalid classroom, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }

        public InvalidClassroomException()
            : base(message: "Invalid classroom. Please fix the errors and try again.") { }
    }
}
