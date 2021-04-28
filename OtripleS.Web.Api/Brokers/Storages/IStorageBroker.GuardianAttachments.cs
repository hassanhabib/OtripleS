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
        public ValueTask<GuardianAttachment> InsertGuardianAttachmentAsync(
           GuardianAttachment guradianAttachment);

        public IQueryable<GuardianAttachment> SelectAllGuardianAttachments();

        public ValueTask<GuardianAttachment> SelectGuardianAttachmentByIdAsync(
            Guid guradianId,
            Guid attachmentId);

        public ValueTask<GuardianAttachment> UpdateGuardianAttachmentAsync(
            GuardianAttachment guradianAttachment);

        public ValueTask<GuardianAttachment> DeleteGuardianAttachmentAsync(
            GuardianAttachment guradianAttachment);
    }
}
