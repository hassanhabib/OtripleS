//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.AssignmentAttachments
{
    public partial class AssignmentAttachmentService
    {
        private void ValidateAssignmentAttachmentOnCreate(AssignmentAttachment assignmentAttachment)
        {
            ValidateAssignmentAttachmentIsNull(assignmentAttachment);
        }

        private void ValidateAssignmentAttachmentIsNull(AssignmentAttachment assignmentContact)
        {
            if (assignmentContact is null)
            {
                throw new NullAssignmentAttachmentException();
            }
        }
    }
}
