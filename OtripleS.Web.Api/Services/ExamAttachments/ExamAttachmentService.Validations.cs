//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.ExamAttachments
{
    public partial class ExamAttachmentService
    {
        public void ValidateExamAttachmentOnCreate(ExamAttachment examAttachment)
        {
            ValidateExamAttachmentIsNull(examAttachment);
            ValidateExamAttachmentIds(examAttachment.ExamId, examAttachment.AttachmentId);
        }

        private void ValidateExamAttachmentIsNull(ExamAttachment examAttachment)
        {
            if (examAttachment is null)
            {
                throw new NullExamAttachmentException();
            }
        }

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
          ExamAttachment storageExamAttachment,
          Guid examId, Guid attachmentId)
        {
            if (storageExamAttachment == null)
                throw new NotFoundExamAttachmentException(examId, attachmentId);
        }

        private void ValidateStorageExamAttachments(IQueryable<ExamAttachment> storageExamAttachments)
        {
            if (storageExamAttachments.Count() == 0)
            {
                this.loggingBroker.LogWarning("No exam attachments found in storage.");
            }
        }

        private bool IsInvalid(Guid input) => input == default;
    }
}
