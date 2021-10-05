// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class LockedCourseAttachmentException : Exception
    {
        public LockedCourseAttachmentException(Exception innerException)
            : base(message: "Locked course attachment record exception, please try again later.", innerException) { }
    }
}
