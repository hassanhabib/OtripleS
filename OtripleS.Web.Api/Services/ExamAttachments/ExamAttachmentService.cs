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
using OtripleS.Web.Api.Models.ExamAttachments;

namespace OtripleS.Web.Api.Services.ExamAttachments
{
    public partial class ExamAttachmentService : IExamAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public ExamAttachmentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<ExamAttachment> AddExamAttachmentAsync(ExamAttachment examAttachment) =>
        TryCatch(async () =>
        {
            ValidateExamAttachmentOnCreate(examAttachment);

            return await this.storageBroker.InsertExamAttachmentAsync(examAttachment);
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
        public IQueryable<ExamAttachment> RetrieveAllExamAttachments() =>
        TryCatch(() =>
        {
            IQueryable<ExamAttachment> storageExamAttachments =
                storageBroker.SelectAllExamAttachments();

            ValidateStorageExamAttachments(storageExamAttachments);

            return storageExamAttachments;

        });


        public ValueTask<ExamAttachment> RetrieveExamAttachmentByIdAsync(
            Guid examId,
            Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateExamAttachmentIds(examId, attachmentId);

            ExamAttachment storageExamAttachment =
                await this.storageBroker.SelectExamAttachmentByIdAsync(examId, attachmentId);

            ValidateStorageExamAttachment(storageExamAttachment, examId, attachmentId);

            return storageExamAttachment;
        });
    }
}
