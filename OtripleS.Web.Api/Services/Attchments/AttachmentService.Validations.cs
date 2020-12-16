// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
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

        private void ValidateAttachmentOnCreate(Attachment Attachment)
        {
            ValidateAttachmentIsNull(Attachment);
        }

        private void ValidateAttachmentIsNull(Attachment attachment)
        {
            if (attachment is null)
            {
                throw new NullAttachmentException();
            }
        }
    }
}