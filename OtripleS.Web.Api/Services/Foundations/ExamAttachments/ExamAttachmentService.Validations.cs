//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;
using System;
using System.Linq;

namespace OtripleS.Web.Api.Services.Foundations.ExamAttachments
{
    public partial class ExamAttachmentService
    {
        public void ValidateExamAttachmentOnAdd(ExamAttachment examAttachment)
        {
            ValidateExamAttachmentIsNull(examAttachment);
            ValidateExamAttachmentIds(examAttachment.ExamId, examAttachment.AttachmentId);
        }

        private static void ValidateExamAttachmentIsNull(ExamAttachment examAttachment)
        {
            if (examAttachment is null)
            {
                throw new NullExamAttachmentException();
            }
        }

        private static void ValidateExamAttachmentIds(Guid examId, Guid attachmentId)
        {
            Validate(
                (Rule: IsInvalid(examId), Parameter: nameof(ExamAttachment.ExamId)),
                (Rule: IsInvalid(attachmentId), Parameter: nameof(ExamAttachment.AttachmentId)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidExamAttachmentException = new InvalidExamAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidExamAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidExamAttachmentException.ThrowIfContainsErrors();
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
            if (!storageExamAttachments.Any())
            {
                this.loggingBroker.LogWarning("No exam attachments found in storage.");
            }
        }
    }
}
