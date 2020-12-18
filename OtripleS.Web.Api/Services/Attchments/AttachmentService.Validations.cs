// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Attachments.Exceptions;

namespace OtripleS.Web.Api.Services.Attachments
{
	public partial class AttachmentService : IAttachmentService
	{
		private void ValidateAttachmentId(Guid attachmentId)
		{
			if (IsInvalid(attachmentId))
			{
				throw new InvalidAttachmentException(
					parameterName: nameof(Attachment.Id),
					parameterValue: attachmentId);
			}
		}

		private void ValidateStorageAttachment(Attachment storageAttachment, Guid attachmentId)
		{
			if (storageAttachment == null)
			{
				throw new NotFoundAttachmentException(attachmentId);
			}
		}

		private bool IsInvalid(Guid input) => input == Guid.Empty;
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

		private void ValidateInvalidAuditFields(Attachment attachment)
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

		private void ValidateInvalidFields(Attachment attachment)
		{
			if (IsInvalid(attachment.Label))
			{
				throw new InvalidAttachmentException(
					parameterName: nameof(Attachment.Label),
					parameterValue: attachment.Label);
			}

			if (IsInvalid(attachment.Description))
			{
				throw new InvalidAttachmentException(
					parameterName: nameof(Attachment.Description),
					parameterValue: attachment.Description);
			}

			if (IsInvalid(attachment.ContectType))
			{
				throw new InvalidAttachmentException(
					parameterName: nameof(Attachment.ContectType),
					parameterValue: attachment.ContectType);
			}

			if (IsInvalid(attachment.Extension))
			{
				throw new InvalidAttachmentException(
					parameterName: nameof(Attachment.Extension),
					parameterValue: attachment.Extension);
			}
		}

		private void ValidateAttachmentIdIsNull(Guid attachmentId)
		{
			if (attachmentId == default)
			{
				throw new InvalidAttachmentException(
					parameterName: nameof(Attachment.Id),
					parameterValue: attachmentId);
			}
		}

		private void ValidateAttachmentIsNull(Attachment attachment)
		{
			if (attachment is null)
			{
				throw new NullAttachmentException();
			}
		}

		private void ValidateStorageAttachments(IQueryable<Attachment> storageAttachments)
		{
			if (storageAttachments.Count() == 0)
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