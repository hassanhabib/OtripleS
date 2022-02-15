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
using OtripleS.Web.Api.Models.StudentAttachments;

namespace OtripleS.Web.Api.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentService : IStudentAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        public StudentAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
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
        TryCatch(() => this.storageBroker.SelectAllStudentAttachments());

        public ValueTask<StudentAttachment> RetrieveStudentAttachmentByIdAsync
            (Guid studentId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateStudentAttachmentIdIsNull(studentId, attachmentId);

            StudentAttachment maybeStudentAttachment =
               await this.storageBroker.SelectStudentAttachmentByIdAsync(studentId, attachmentId);

            ValidateStorageStudentAttachment(maybeStudentAttachment, studentId, attachmentId);

            return maybeStudentAttachment;
        });

    }
}
