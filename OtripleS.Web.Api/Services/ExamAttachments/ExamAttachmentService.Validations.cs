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
            ValidateExamAttachmentIdIsNull(examAttachment.ExamId, examAttachment.AttachmentId);
        }

        private void ValidateExamAttachmentIsNull(ExamAttachment examAttachment)
        {
            if (examAttachment is null)
            {
                throw new NullExamAttachmentException();
            }
        }

        private void ValidateExamAttachmentIdIsNull(Guid examId, Guid attachmentId)
        {
            if (examId == default)
            {
                throw new InvalidExamAttachmentException(
                    parameterName: nameof(ExamAttachment.ExamId),
                    parameterValue: examId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidExamAttachmentException(
                    parameterName: nameof(ExamAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private void ValidateStorageExamAttachments(IQueryable<ExamAttachment> storageExamAttachments)
        {
            if (storageExamAttachments.Count() == 0)
            {
                this.loggingBroker.LogWarning("No exam attachments found in storage.");
            }
        }
    }
}
