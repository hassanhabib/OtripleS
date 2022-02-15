// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class InvalidCourseAttachmentException : Xeption
    {
        public InvalidCourseAttachmentException(string parameterName, object parameterValue)
            : base(message: $"Invalid Course Attachment, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }

        public InvalidCourseAttachmentException()
            : base(message: "Invalid course attachment. Please fix the errors and try again.") { }
    }
}
