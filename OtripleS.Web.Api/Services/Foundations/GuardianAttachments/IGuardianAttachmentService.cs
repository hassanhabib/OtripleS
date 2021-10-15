//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Models.GuardianAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.GuardianAttachments
{
    public interface IGuardianAttachmentService
    {
        ValueTask<GuardianAttachment> AddGuardianAttachmentAsync(GuardianAttachment guardianAttachment);
        IQueryable<GuardianAttachment> RetrieveAllGuardianAttachments();

        ValueTask<GuardianAttachment> RetrieveGuardianAttachmentByIdAsync
            (Guid guardianId, Guid attachmentId);

        ValueTask<GuardianAttachment> RemoveGuardianAttachmentByIdAsync(Guid guardianId, Guid attachmentId);
    }
}
