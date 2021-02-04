//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.CourseAttachments
{
    public partial class CourseAttachmentService
    {
        private void ValidateCourseAttachmentOnCreate(CourseAttachment courseAttachment)
        {
            ValidateCourseAttachmentIsNull(courseAttachment);

            ValidateCourseAttachmentIds(courseAttachment);

        }

        private void ValidateCourseAttachmentIsNull(CourseAttachment courseAttachment)
        {
            if (courseAttachment is null)
            {
                throw new NullCourseAttachmentException();
            }
        }

        private void ValidateCourseAttachmentIds(CourseAttachment courseAttachment)
        {
            switch (courseAttachment)
            {
                case { } when courseAttachment.CourseId == default:
                    throw new InvalidCourseAttachmentException(
                        parameterName: nameof(CourseAttachment.CourseId),
                        parameterValue: courseAttachment.CourseId);

                case { } when courseAttachment.AttachmentId == default:
                    throw new InvalidCourseAttachmentException(
                        parameterName: nameof(CourseAttachment.AttachmentId),
                        parameterValue: courseAttachment.AttachmentId);
        }

        private void ValidateStorageCourseAttachment(
            CourseAttachment storageCourseAttachment, 
            Guid courseId, 
            Guid attachmentId)
        {
            if (storageCourseAttachment == null)
                throw new NotFoundCourseAttachmentException(courseId, attachmentId);
        }
    }
}
