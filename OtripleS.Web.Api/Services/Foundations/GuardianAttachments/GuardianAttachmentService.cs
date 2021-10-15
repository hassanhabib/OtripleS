//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.GuardianAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.GuardianAttachments
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

        public ValueTask<GuardianAttachment> AddGuardianAttachmentAsync(GuardianAttachment guardianAttachment) =>
            TryCatch(async () =>
        {
            ValidateGuardianAttachmentOnCreate(guardianAttachment);

            return await this.storageBroker.InsertGuardianAttachmentAsync(guardianAttachment);
        });

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

        public ValueTask<GuardianAttachment> RemoveGuardianAttachmentByIdAsync(Guid guardianId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateGuardianAttachmentIdIsNull(guardianId, attachmentId);

            GuardianAttachment mayBeGuardianAttachment =
               await this.storageBroker.SelectGuardianAttachmentByIdAsync(guardianId, attachmentId);

            ValidateStorageGuardianAttachment(mayBeGuardianAttachment, guardianId, attachmentId);

            return await this.storageBroker.DeleteGuardianAttachmentAsync(mayBeGuardianAttachment);
        });
    }
}
