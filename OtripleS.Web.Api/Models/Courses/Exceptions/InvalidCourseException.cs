// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Courses.Exceptions
{
    public class InvalidCourseException : Xeption
    {
        public InvalidCourseException(string parameterName, object parameterValue)
            : base(message: $"Invalid course, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }

        public InvalidCourseException()
            : base(message: "Invalid course. Please fix the errors and try again.")
        { }
    }
}
