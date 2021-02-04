// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class NotFoundCourseAttachmentException : Exception
    {
        public NotFoundCourseAttachmentException(Guid courseId, Guid attachmentId)
          : base($"Couldn't find CourseAttachment with courseId: " +
                    $"{courseId} " +
                    $"and attachmentId: {attachmentId}.")
        { }
    }
}
