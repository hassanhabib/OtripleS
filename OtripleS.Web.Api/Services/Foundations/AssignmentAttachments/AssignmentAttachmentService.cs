//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentService : IAssignmentAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AssignmentAttachmentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<AssignmentAttachment> AddAssignmentAttachmentAsync
            (AssignmentAttachment assignmentAttachment) =>
        TryCatch(async () =>
        {
            ValidateAssignmentAttachmentOnCreate(assignmentAttachment);

            return await this.storageBroker.InsertAssignmentAttachmentAsync(assignmentAttachment);
        });

        public IQueryable<AssignmentAttachment> RetrieveAllAssignmentAttachments() =>
        TryCatch(() =>
        {
            IQueryable<AssignmentAttachment> storageAssignmentAttachments
                = this.storageBroker.SelectAllAssignmentAttachments();

            ValidateStorageAssignmentAttachments(storageAssignmentAttachments);

            return storageAssignmentAttachments;

        });

        public ValueTask<AssignmentAttachment> RetrieveAssignmentAttachmentByIdAsync(
            Guid assignmentId,
            Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateAssignmentAttachmentIds(assignmentId, attachmentId);

            AssignmentAttachment storageAssignmentAttachment =
                await this.storageBroker.SelectAssignmentAttachmentByIdAsync(assignmentId, attachmentId);

            ValidateStorageAssignmentAttachment(storageAssignmentAttachment, assignmentId, attachmentId);

            return storageAssignmentAttachment;
        });

        public ValueTask<AssignmentAttachment> RemoveAssignmentAttachmentByIdAsync(
            Guid assignmentId,
            Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateAssignmentAttachmentIds(assignmentId, attachmentId);

            AssignmentAttachment maybeAssignmentAttachment =
                await this.storageBroker.SelectAssignmentAttachmentByIdAsync(assignmentId, attachmentId);

            ValidateStorageAssignmentAttachment(maybeAssignmentAttachment, assignmentId, attachmentId);

            return await this.storageBroker.DeleteAssignmentAttachmentAsync(maybeAssignmentAttachment);
        });
    }
}
