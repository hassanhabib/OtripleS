//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.GuardianAttachments;

namespace OtripleS.Web.Api.Services.GuardianAttachmets
{
    public interface IGuardianAttachmentService
    {
        ValueTask<GuardianAttachment> AddGuardianAttachmentAsync(GuardianAttachment guardianAttachment);
        IQueryable<GuardianAttachment> RetrieveAllGuardianAttachments();

        ValueTask<GuardianAttachment> RetrieveGuardianAttachmentByIdAsync
            (Guid guardianId, Guid attachmentId);
    }
}
