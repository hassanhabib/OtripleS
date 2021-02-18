// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.ExamAttachments
{
    public partial class ExamAttachmentService
    {
        private void ValidateExamAttachmentIds(Guid calendarEntryId, Guid attachmentId)
        {
            if (calendarEntryId == default)
            {
                throw new InvalidExamAttachmentException(
                    parameterName: nameof(ExamAttachment.ExamId),
                    parameterValue: calendarEntryId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidExamAttachmentException(
                    parameterName: nameof(ExamAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageExamAttachment(
          ExamAttachment storageCourseAttachment,
          Guid courseId, Guid attachmentId)
        {
            if (storageCourseAttachment == null)
                throw new NotFoundExamAttachmentException(courseId, attachmentId);
        }
    }
}
