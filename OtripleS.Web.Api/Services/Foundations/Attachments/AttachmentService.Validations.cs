// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Attachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Attachments
{
    public partial class AttachmentService : IAttachmentService
    {
        private static void ValidateAttachmentId(Guid attachmentId)
        {
            if (IsInvalid(attachmentId))
            {
                throw new InvalidAttachmentException(
                    parameterName: nameof(Attachment.Id),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageAttachment(Attachment storageAttachment, Guid attachmentId)
        {
            if (storageAttachment == null)
            {
                throw new NotFoundAttachmentException(attachmentId);
            }
        }

        private static void ValidateAgainstStorageAttachmentOnModify(
            Attachment inputAttachment,
            Attachment storageAttachment)
        {
            switch (inputAttachment)
            {
                case { } when inputAttachment.CreatedDate != storageAttachment.CreatedDate:
                    throw new InvalidAttachmentException(
                        parameterName: nameof(Attachment.CreatedDate),
                        parameterValue: inputAttachment.CreatedDate);

                case { } when inputAttachment.CreatedBy != storageAttachment.CreatedBy:
                    throw new InvalidAttachmentException(
                        parameterName: nameof(Attachment.CreatedBy),
                        parameterValue: inputAttachment.CreatedBy);

                case { } when inputAttachment.UpdatedDate == storageAttachment.UpdatedDate:
                    throw new InvalidAttachmentException(
                        parameterName: nameof(Attachment.UpdatedDate),
                        parameterValue: inputAttachment.UpdatedDate);
            }
        }

        private static bool IsInvalid(Guid input) => input == Guid.Empty;
        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(DateTimeOffset input) => input == default;

        private void ValidateAttachmentOnCreate(Attachment attachment)
        {
            ValidateAttachmentIsNull(attachment);
            ValidateAttachmentIdIsNull(attachment.Id);
            ValidateInvalidFields(attachment);
            ValidateInvalidAuditFields(attachment);
            ValidateAuditFieldsDataOnCreate(attachment);
        }

        private void ValidateAttachmentOnModify(Attachment attachment)
        {
            ValidateAttachmentIsNull(attachment);
            ValidateAttachmentIdIsNull(attachment.Id);
            ValidateInvalidFields(attachment);
            ValidateInvalidAuditFields(attachment);
            ValidateDatesAreNotSame(attachment);
            ValidateUpdatedDateIsRecent(attachment);
        }

        private void ValidateAuditFieldsDataOnCreate(Attachment attachment)
        {
            switch (attachment)
            {
                case { } when attachment.UpdatedBy != attachment.CreatedBy:
                    throw new InvalidAttachmentException(
                    parameterName: nameof(Attachment.UpdatedBy),
                    parameterValue: attachment.UpdatedBy);

                case { } when attachment.UpdatedDate != attachment.CreatedDate:
                    throw new InvalidAttachmentException(
                    parameterName: nameof(Attachment.UpdatedDate),
                    parameterValue: attachment.UpdatedDate);

                case { } when IsDateNotRecent(attachment.CreatedDate):
                    throw new InvalidAttachmentException(
                    parameterName: nameof(Attachment.CreatedDate),
                    parameterValue: attachment.CreatedDate);
            }
        }

        private static void ValidateDatesAreNotSame(Attachment attachment)
        {
            if (attachment.CreatedDate == attachment.UpdatedDate)
            {
                throw new InvalidAttachmentException(
                    parameterName: nameof(Attachment.UpdatedDate),
                    parameterValue: attachment.UpdatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(Attachment attachment)
        {
            if (IsDateNotRecent(attachment.UpdatedDate))
            {
                throw new InvalidAttachmentException(
                    parameterName: nameof(attachment.UpdatedDate),
                    parameterValue: attachment.UpdatedDate);
            }
        }

        private static void ValidateInvalidAuditFields(Attachment attachment)
        {
            switch (attachment)
            {
                case { } when IsInvalid(attachment.CreatedBy):
                    throw new InvalidAttachmentException(
                    parameterName: nameof(Attachment.CreatedBy),
                    parameterValue: attachment.CreatedBy);

                case { } when IsInvalid(attachment.CreatedDate):
                    throw new InvalidAttachmentException(
                    parameterName: nameof(Attachment.CreatedDate),
                    parameterValue: attachment.CreatedDate);

                case { } when IsInvalid(attachment.UpdatedBy):
                    throw new InvalidAttachmentException(
                    parameterName: nameof(Attachment.UpdatedBy),
                    parameterValue: attachment.UpdatedBy);

                case { } when IsInvalid(attachment.UpdatedDate):
                    throw new InvalidAttachmentException(
                    parameterName: nameof(Attachment.UpdatedDate),
                    parameterValue: attachment.UpdatedDate);
            }
        }

        private static void ValidateInvalidFields(Attachment attachment)
        {
            switch (attachment)
            {
                case { } when IsInvalid(attachment.Label):
                    throw new InvalidAttachmentException(
                        parameterName: nameof(Attachment.Label),
                        parameterValue: attachment.Label);

                case { } when IsInvalid(attachment.Description):
                    throw new InvalidAttachmentException(
                        parameterName: nameof(Attachment.Description),
                        parameterValue: attachment.Description);

                case { } when IsInvalid(attachment.ContectType):
                    throw new InvalidAttachmentException(
                        parameterName: nameof(Attachment.ContectType),
                        parameterValue: attachment.ContectType);

                case { } when IsInvalid(attachment.Extension):
                    throw new InvalidAttachmentException(
                        parameterName: nameof(Attachment.Extension),
                        parameterValue: attachment.Extension);

            }
        }

        private static void ValidateAttachmentIdIsNull(Guid attachmentId)
        {
            if (attachmentId == default)
            {
                throw new InvalidAttachmentException(
                    parameterName: nameof(Attachment.Id),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateAttachmentIsNull(Attachment attachment)
        {
            if (attachment is null)
            {
                throw new NullAttachmentException();
            }
        }

        private void ValidateStorageAttachments(IQueryable<Attachment> storageAttachments)
        {
            if (!storageAttachments.Any())
            {
                this.loggingBroker.LogWarning("No attachments found in storage.");
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}