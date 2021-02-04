// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.CourseAttachments
{
    public partial class CourseAttachmentService
    {
        private void ValidateCourseAttachmentIds(Guid courseId, Guid attachmentId)
        {
            if (courseId == default)
            {
                throw new InvalidCourseAttachmentException(
                    parameterName: nameof(CourseAttachment.CourseId),
                    parameterValue: courseId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidCourseAttachmentException(
                    parameterName: nameof(CourseAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageCourseAttachment(
          CourseAttachment storageCourseAttachment,
          Guid courseId, Guid attachmentId)
        {
            if (storageCourseAttachment == null)
                throw new NotFoundCourseAttachmentException(courseId, attachmentId);
        }
    }
}
