//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.TeacherContacts;

namespace OtripleS.Web.Api.Services.TeacherContacts
{
    public partial class TeacherContactService : ITeacherContactService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public TeacherContactService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<TeacherContact> AddTeacherContactAsync(TeacherContact teacherContact) =>
        TryCatch(() =>
        {
            ValidateTeacherContactOnAdd(teacherContact);

            return this.storageBroker.InsertTeacherContactAsync(teacherContact);
        });

        public IQueryable<TeacherContact> RetrieveAllTeacherContacts() =>
        TryCatch(() =>
        {
            IQueryable<TeacherContact> storageTeacherContacts =
                this.storageBroker.SelectAllTeacherContacts();

            ValidateStorageTeacherContacts(storageTeacherContacts);

            return storageTeacherContacts;
        });

        public ValueTask<TeacherContact> RemoveTeacherContactByIdAsync(Guid teacherId, Guid contactId) =>
        TryCatch(async () =>
        {
            ValidateTeacherContactIdIsNull(teacherId, contactId);

            TeacherContact mayBeTeacherContact =
                await this.storageBroker.SelectTeacherContactByIdAsync(teacherId, contactId);

            ValidateStorageTeacherContact(mayBeTeacherContact, teacherId, contactId);

            return await this.storageBroker.DeleteTeacherContactAsync(mayBeTeacherContact);
        });

        public ValueTask<TeacherContact> RetrieveTeacherContactByIdAsync(Guid teacherId, Guid contactId) =>
        TryCatch(async () =>
        {
            ValidateTeacherContactIdIsNull(teacherId, contactId);

            TeacherContact storageTeacherContact =
                await this.storageBroker.SelectTeacherContactByIdAsync(teacherId, contactId);

            ValidateStorageTeacherContact(storageTeacherContact, teacherId, contactId);

            return storageTeacherContact;
        });
    }
}