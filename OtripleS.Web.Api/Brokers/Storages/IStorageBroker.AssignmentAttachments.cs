// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.AssignmentAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<AssignmentAttachment> InsertAssignmentAttachmentAsync(
           AssignmentAttachment assignmentEntryAttachment);

        IQueryable<AssignmentAttachment> SelectAllAssignmentAttachments();

        ValueTask<AssignmentAttachment> SelectAssignmentAttachmentByIdAsync(
            Guid assignmentId,
            Guid attachmentId);

        ValueTask<AssignmentAttachment> UpdateAssignmentAttachmentAsync(
            AssignmentAttachment assignmentEntryAttachment);

        ValueTask<AssignmentAttachment> DeleteAssignmentAttachmentAsync(
            AssignmentAttachment assignmentEntryAttachment);
    }
}