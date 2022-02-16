// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class CourseAttachmentValidationException : Exception
    {
        public CourseAttachmentValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
    }
}
