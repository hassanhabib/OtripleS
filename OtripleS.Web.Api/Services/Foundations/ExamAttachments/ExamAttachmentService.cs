// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.ExamAttachments;

namespace OtripleS.Web.Api.Services.Foundations.ExamAttachments
{
    public partial class ExamAttachmentService : IExamAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ExamAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ExamAttachment> AddExamAttachmentAsync(ExamAttachment examAttachment) =>
        TryCatch(async () =>
        {
            ValidateExamAttachmentOnAdd(examAttachment);

            return await this.storageBroker.InsertExamAttachmentAsync(examAttachment);
        });

        public IQueryable<ExamAttachment> RetrieveAllExamAttachments() =>
        TryCatch(() => storageBroker.SelectAllExamAttachments());

        public ValueTask<ExamAttachment> RetrieveExamAttachmentByIdAsync(
            Guid examId,
            Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateExamAttachmentIds(examId, attachmentId);

            ExamAttachment maybeExamAttachment =
                await this.storageBroker.SelectExamAttachmentByIdAsync(examId, attachmentId);

            ValidateStorageExamAttachment(maybeExamAttachment, examId, attachmentId);

            return maybeExamAttachment;
        });

        public ValueTask<ExamAttachment> RemoveExamAttachmentByIdAsync(
           Guid examId,
           Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateExamAttachmentIds(examId, attachmentId);

            ExamAttachment maybeExamAttachment =
              await this.storageBroker.SelectExamAttachmentByIdAsync(examId, attachmentId);

            ValidateStorageExamAttachment(maybeExamAttachment, examId, attachmentId);

            return await this.storageBroker.DeleteExamAttachmentAsync(maybeExamAttachment);
        });
    }
}
