// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.GuardianAttachments
{
    public partial class GuardianAttachmentService
    {
        private static void ValidateGuardianAttachmentOnCreate(GuardianAttachment guardianAttachment)
        {
            ValidateGuardianAttachmentIsNull(guardianAttachment);
            ValidateGuardianAttachmentIdIsNull(guardianAttachment.GuardianId, guardianAttachment.AttachmentId);
        }

        private static void ValidateGuardianAttachmentIsNull(GuardianAttachment guardianAttachment)
        {
            if (guardianAttachment is null)
            {
                throw new NullGuardianAttachmentException();
            }
        }

        private static void ValidateGuardianAttachmentIdIsNull(Guid guardianId, Guid attachmentId)
        {
            if (guardianId == default)
            {
                throw new InvalidGuardianAttachmentException(
                    parameterName: nameof(GuardianAttachment.GuardianId),
                    parameterValue: guardianId);
            }
            else if (attachmentId == default)
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
            if (storageStudentAttachment is null)
            {
                throw new NotFoundGuardianAttachmentException(guardianId, attachmentId);
            }
        }
    }
}
