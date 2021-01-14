//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.GuardianAttachmets
{
    public partial class GuardianAttachmentService
    {
        private void ValidateGuardianAttachmentIdIsNull(Guid guardianId, Guid attachmentId)
        {
            if (guardianId == default)
            {
                throw new InvalidGuardianAttachmentException(
                    parameterName: nameof(GuardianAttachment.GuardianId),
                    parameterValue: guardianId);
            }

            if (attachmentId == default)
            {
                throw new InvalidGuardianAttachmentException(
                    parameterName: nameof(GuardianAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageGuardianAttachment(
            GuardianAttachment storageStudentAttachment,
            Guid guardianId, Guid attachmentId)
        {
            if (storageStudentAttachment == null)
            {
                throw new NotFoundGuardianAttachmentException(guardianId, attachmentId);
            }
        }

        private void ValidateStorageGuardianAttachments(IQueryable<GuardianAttachment> storageGuardianAttachments)
        {
            if (storageGuardianAttachments.Count() == 0)
            {
                this.loggingBroker.LogWarning("No guardian attachments found in storage.");
            }
        }
    }
}
