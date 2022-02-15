// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.AssignmentAttachments;

namespace OtripleS.Web.Api.Services.Foundations.AssignmentAttachments
{
    public interface IAssignmentAttachmentService
    {
        ValueTask<AssignmentAttachment> RetrieveAssignmentAttachmentByIdAsync(
            Guid assignmentId,
            Guid attachmentId);

        ValueTask<AssignmentAttachment> AddAssignmentAttachmentAsync(AssignmentAttachment assignmentAttachment);
        IQueryable<AssignmentAttachment> RetrieveAllAssignmentAttachments();

        ValueTask<AssignmentAttachment> RemoveAssignmentAttachmentByIdAsync(
            Guid assignmentId,
            Guid attachmentId);
    }
}
