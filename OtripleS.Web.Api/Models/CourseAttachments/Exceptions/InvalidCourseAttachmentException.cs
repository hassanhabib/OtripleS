// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class InvalidCourseAttachmentException : Exception
    {
        public InvalidCourseAttachmentException(string parameterName, object parameterValue)
            : base($"Invalid Course Attachment, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
