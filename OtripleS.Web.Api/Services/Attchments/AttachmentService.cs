// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Attachments;

namespace OtripleS.Web.Api.Services.Attachments
{
    public partial class AttachmentService : IAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AttachmentService(IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Attachment> InsertAttachmentAsync(Attachment attachment)
        {
            return this.storageBroker.InsertAttachmentAsync(attachment);
        }

        public ValueTask<Attachment> RetrieveAttachmentByIdAsync(Guid attachmentId) =>
        TryCatch(async () => {
            ValidateAttachmentId(attachmentId);

            Attachment storageAttachment = await this.storageBroker.SelectAttachmentByIdAsync(attachmentId);
            ValidateStorageAttachment(storageAttachment, attachmentId);

            return storageAttachment;
        });

    }
}