//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Foundations.StudentAttachments;

namespace OtripleS.Web.Api.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentService : IStudentAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public StudentAttachmentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<StudentAttachment> AddStudentAttachmentAsync(StudentAttachment studentAttachment) =>
        TryCatch(async () =>
        {
            ValidateStudentAttachmentOnCreate(studentAttachment);

            return await this.storageBroker.InsertStudentAttachmentAsync(studentAttachment);
        });

        public ValueTask<StudentAttachment> RemoveStudentAttachmentByIdAsync(Guid studentId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateStudentAttachmentIdIsNull(studentId, attachmentId);

            StudentAttachment mayBeStudentAttachment =
                await this.storageBroker.SelectStudentAttachmentByIdAsync(studentId, attachmentId);

            ValidateStorageStudentAttachment(mayBeStudentAttachment, studentId, attachmentId);

            return await this.storageBroker.DeleteStudentAttachmentAsync(mayBeStudentAttachment);
        });

        public IQueryable<StudentAttachment> RetrieveAllStudentAttachments() =>
        TryCatch(() =>
        {
            IQueryable<StudentAttachment> storageStudentAttachments = this.storageBroker.SelectAllStudentAttachments();

            ValidateStorageStudentAttachments(storageStudentAttachments);

            return storageStudentAttachments;
        });

        public ValueTask<StudentAttachment> RetrieveStudentAttachmentByIdAsync
            (Guid studentId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateStudentAttachmentIdIsNull(studentId, attachmentId);

            StudentAttachment storageStudentAttachment =
               await this.storageBroker.SelectStudentAttachmentByIdAsync(studentId, attachmentId);

            ValidateStorageStudentAttachment(storageStudentAttachment, studentId, attachmentId);

            return storageStudentAttachment;
        });

    }
}
