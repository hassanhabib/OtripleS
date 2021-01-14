//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.GuardianAttachments;

namespace OtripleS.Web.Api.Services.GuardianAttachmets
{
    public partial class GuardianAttachmentService : IGuardianAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public GuardianAttachmentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public IQueryable<GuardianAttachment> RetrieveAllGuardianAttachments() =>
        TryCatch(() =>
        {
            IQueryable<GuardianAttachment> storageGuardianAttachments = this.storageBroker.SelectAllGuardianAttachments();

            ValidateStorageGuardianAttachments(storageGuardianAttachments);

            return storageGuardianAttachments;
        });

        public ValueTask<GuardianAttachment> RetrieveGuardianAttachmentByIdAsync
            (Guid guardianId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateGuardianAttachmentIdIsNull(guardianId, attachmentId);

            GuardianAttachment storageGuardianAttachment =
               await this.storageBroker.SelectGuardianAttachmentByIdAsync(guardianId, attachmentId);

            ValidateStorageGuardianAttachment(storageGuardianAttachment, guardianId, attachmentId);

            return storageGuardianAttachment;
        });

    }
}
