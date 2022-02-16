﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.AssignmentAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<AssignmentAttachment> InsertAssignmentAttachmentAsync(
           AssignmentAttachment assignmentAttachment);

        IQueryable<AssignmentAttachment> SelectAllAssignmentAttachments();

        ValueTask<AssignmentAttachment> SelectAssignmentAttachmentByIdAsync(
            Guid assignmentId,
            Guid attachmentId);

        ValueTask<AssignmentAttachment> UpdateAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment);

        ValueTask<AssignmentAttachment> DeleteAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment);
    }
}