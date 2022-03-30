// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.AssignmentAttachments;

namespace OtripleS.Web.Api.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentService : IAssignmentAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        public AssignmentAttachmentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }
        public ValueTask<AssignmentAttachment> AddAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment) => TryCatch(async () =>
        {
            ValidateAssignmentAttachmentOnCreate(assignmentAttachment);

            return await this.storageBroker.InsertAssignmentAttachmentAsync(assignmentAttachment);
        });
        public IQueryable<AssignmentAttachment> RetrieveAllAssignmentAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllAssignmentAttachments());
        public ValueTask<AssignmentAttachment> RetrieveAssignmentAttachmentByIdAsync(
            Guid assignmentId,
            Guid attachmentId) => TryCatch(async () =>
        {
            ValidateAssignmentAttachmentIds(assignmentId, attachmentId);

            AssignmentAttachment storageAssignmentAttachment =
                await this.storageBroker.SelectAssignmentAttachmentByIdAsync(assignmentId, attachmentId);

            ValidateStorageAssignmentAttachment(storageAssignmentAttachment, assignmentId, attachmentId);

            return storageAssignmentAttachment;
        });
        public ValueTask<AssignmentAttachment> RemoveAssignmentAttachmentByIdAsync(
            Guid assignmentId,
            Guid attachmentId) => TryCatch(async () =>
        {
            ValidateAssignmentAttachmentIds(assignmentId, attachmentId);

            AssignmentAttachment maybeAssignmentAttachment =
                await this.storageBroker.SelectAssignmentAttachmentByIdAsync(assignmentId, attachmentId);

            ValidateStorageAssignmentAttachment(maybeAssignmentAttachment, assignmentId, attachmentId);

            return await this.storageBroker.DeleteAssignmentAttachmentAsync(maybeAssignmentAttachment);
        });
    }
}
