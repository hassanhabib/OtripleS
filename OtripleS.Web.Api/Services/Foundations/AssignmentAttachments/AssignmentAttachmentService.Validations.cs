//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentService
    {
        private static void ValidateAssignmentAttachmentOnCreate(AssignmentAttachment assignmentAttachment)
        {
            ValidateAssignmentAttachmentIsNull(assignmentAttachment);

            ValidateAssignmentAttachmentIds(
                assignmentAttachment.AssignmentId,
                assignmentAttachment.AttachmentId);
        }

        private static void ValidateAssignmentAttachmentIsNull(AssignmentAttachment assignmentContact)
        {
            if (assignmentContact is null)
            {
                throw new NullAssignmentAttachmentException();
            }
        }

        private static void ValidateAssignmentAttachmentIds(Guid assignmentId, Guid attachmentId)
        {
            if (assignmentId == default)
            {
                throw new InvalidAssignmentAttachmentException(
                    parameterName: nameof(AssignmentAttachment.AssignmentId),
                    parameterValue: assignmentId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidAssignmentAttachmentException(
                    parameterName: nameof(AssignmentAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageAssignmentAttachment(
            AssignmentAttachment storageAssignmentAttachment,
            Guid assignmentId, Guid attachmentId)
        {
            if (storageAssignmentAttachment == null)
                throw new NotFoundAssignmentAttachmentException(assignmentId, attachmentId);
        }

        private void ValidateStorageAssignmentAttachments
            (IQueryable<AssignmentAttachment> storageAssignmentAttachments)
        {
            if (!storageAssignmentAttachments.Any())
            {
                this.loggingBroker.LogWarning("No assignment attachments found in storage.");
            }
        }

    }
}
