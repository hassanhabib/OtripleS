// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.GuardianAttachments;

namespace OtripleS.Web.Api.Services.Foundations.GuardianAttachments
{
    public partial class GuardianAttachmentService : IGuardianAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public GuardianAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<GuardianAttachment> AddGuardianAttachmentAsync(GuardianAttachment guardianAttachment) =>
            TryCatch(async () =>
        {
            ValidateGuardianAttachmentOnCreate(guardianAttachment);

            return await this.storageBroker.InsertGuardianAttachmentAsync(guardianAttachment);
        });

        public IQueryable<GuardianAttachment> RetrieveAllGuardianAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllGuardianAttachments());

        public ValueTask<GuardianAttachment> RetrieveGuardianAttachmentByIdAsync
            (Guid guardianId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateGuardianAttachmentIdIsNull(guardianId, attachmentId);

            GuardianAttachment maybeGuardianAttachment =
               await this.storageBroker.SelectGuardianAttachmentByIdAsync(guardianId, attachmentId);

            ValidateStorageGuardianAttachment(maybeGuardianAttachment, guardianId, attachmentId);

            return maybeGuardianAttachment;
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
