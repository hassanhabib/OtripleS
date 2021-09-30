// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.CourseAttachments
{
    public partial class CourseAttachmentService
    {
        private static void ValidateCourseAttachmentOnAdd(CourseAttachment courseAttachment)
        {
            ValidateCourseAttachmentIsNull(courseAttachment);

            Validate(
                (Rule: IsInvalid(courseAttachment.CourseId), Parameter: nameof(CourseAttachment.CourseId)),
                (Rule: IsInvalid(courseAttachment.AttachmentId), Parameter: nameof(CourseAttachment.AttachmentId)));
        }

        private static void ValidateCourseAttachmentIsNull(CourseAttachment courseAttachment)
        {
            if (courseAttachment is null)
            {
                throw new NullCourseAttachmentException();
            }
        }

        private static void ValidateCourseAttachmentIds(Guid courseId, Guid attachmentId)
        {
            Validate(
               (Rule: IsInvalid(courseId), Parameter: nameof(CourseAttachment.CourseId)),
               (Rule: IsInvalid(attachmentId), Parameter: nameof(CourseAttachment.AttachmentId)));
        }

        private static void ValidateStorageCourseAttachment(
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

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidCourseAttachmentException = new InvalidCourseAttachmentException();

            foreach((dynamic rule, string parameter) in validations)
            {
                if(rule.Condition)
                {
                    invalidCourseAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidCourseAttachmentException.ThrowIfContainsErrors();
        }
    }
}
