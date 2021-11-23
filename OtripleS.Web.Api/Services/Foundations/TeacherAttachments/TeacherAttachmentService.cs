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
using OtripleS.Web.Api.Models.TeacherAttachments;

namespace OtripleS.Web.Api.Services.Foundations.TeacherAttachments
{
    public partial class TeacherAttachmentService : ITeacherAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public TeacherAttachmentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<TeacherAttachment> AddTeacherAttachmentAsync(TeacherAttachment teacherAttachment) =>
        TryCatch(async () =>
        {
            ValidateTeacherAttachmentOnCreate(teacherAttachment);

            return await this.storageBroker.InsertTeacherAttachmentAsync(teacherAttachment);
        });

        public IQueryable<TeacherAttachment> RetrieveAllTeacherAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllTeacherAttachments());

        public ValueTask<TeacherAttachment> RetrieveTeacherAttachmentByIdAsync(
            Guid teacherId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateTeacherAttachmentIds(teacherId, attachmentId);

            TeacherAttachment maybeTeacherAttachment =
               await this.storageBroker.SelectTeacherAttachmentByIdAsync(teacherId, attachmentId);

            ValidateStorageTeacherAttachment(maybeTeacherAttachment, teacherId, attachmentId);

            return maybeTeacherAttachment;
        });

        public ValueTask<TeacherAttachment> RemoveTeacherAttachmentByIdAsync(Guid teacherId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateTeacherAttachmentIds(teacherId, attachmentId);

            TeacherAttachment maybeTeacherAttachment =
               await this.storageBroker.SelectTeacherAttachmentByIdAsync(teacherId, attachmentId);

            ValidateStorageTeacherAttachment(maybeTeacherAttachment, teacherId, attachmentId);

            return await this.storageBroker.DeleteTeacherAttachmentAsync(maybeTeacherAttachment);
        });
    }
}
