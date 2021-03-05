//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.AssignmentAttachments
{
    public partial class AssignmentAttachmentService
    {
        private void ValidateAssignmentAttachmentIds(Guid assignmentId, Guid attachmentId)
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
          AssignmentAttachment storageCourseAttachment,
          Guid courseId, Guid attachmentId)
        {
            if (storageCourseAttachment == null)
                throw new NotFoundAssignmentAttachmentException(courseId, attachmentId);
        }
    }
}
