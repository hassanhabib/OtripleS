// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.GuardianAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<GuardianAttachment> InsertGuardianAttachmentAsync(
          GuardianAttachment guradianAttachment);

        IQueryable<GuardianAttachment> SelectAllGuardianAttachments();

        ValueTask<GuardianAttachment> SelectGuardianAttachmentByIdAsync(
           Guid guradianId,
           Guid attachmentId);

        ValueTask<GuardianAttachment> UpdateGuardianAttachmentAsync(
           GuardianAttachment guradianAttachment);

        ValueTask<GuardianAttachment> DeleteGuardianAttachmentAsync(
           GuardianAttachment guradianAttachment);
    }
}