// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.GuardianAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<GuardianAttachment> InsertGuardianAttachmentAsync(
          GuardianAttachment guardianAttachment);

        IQueryable<GuardianAttachment> SelectAllGuardianAttachments();

        ValueTask<GuardianAttachment> SelectGuardianAttachmentByIdAsync(
           Guid guardianId,
           Guid attachmentId);

        ValueTask<GuardianAttachment> UpdateGuardianAttachmentAsync(
           GuardianAttachment guardianAttachment);

        ValueTask<GuardianAttachment> DeleteGuardianAttachmentAsync(
           GuardianAttachment guardianAttachment);
    }
}