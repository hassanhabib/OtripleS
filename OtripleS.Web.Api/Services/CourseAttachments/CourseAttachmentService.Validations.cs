// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.CourseAttachments
{
    public partial class CourseAttachmentService
    {
        private void ValidateCourseAttachmentOnCreate(CourseAttachment courseAttachment)
        {
            ValidateCourseAttachmentIsNull(courseAttachment);

            ValidateCourseAttachmentIds(courseAttachment.CourseId, courseAttachment.AttachmentId);

        }

        private void ValidateCourseAttachmentIsNull(CourseAttachment courseAttachment)
        {
            if (courseAttachment is null)
            {
                throw new NullCourseAttachmentException();
            }
        }

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

        private void ValidateStorageCourseAttachment(
            CourseAttachment storageCourseAttachment,
            Guid courseId,
            Guid attachmentId)
        {
            if (storageCourseAttachment == null)
                throw new NotFoundCourseAttachmentException(courseId, attachmentId);
        }

        private void ValidateStorageCourseAttachments(
            IQueryable<CourseAttachment> storageCourseAttachments)
        {
            if (!storageCourseAttachments.Any())
            {
                this.loggingBroker.LogWarning("No course attachments found in storage.");
            }
        }
    }
}
