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
using OtripleS.Web.Api.Models.StudentAttachments;

namespace OtripleS.Web.Api.Services.StudentAttachments
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

        public IQueryable<StudentAttachment> RetrieveAllStudentAttachments()
        {
            IQueryable<StudentAttachment> storageStudentAttachments = this.storageBroker.SelectAllStudentAttachments();

            ValidateStorageStudentAttachments(storageStudentAttachments);

            return storageStudentAttachments;
        }

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
