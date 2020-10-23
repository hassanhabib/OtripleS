//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentContacts;

namespace OtripleS.Web.Api.Services.StudentContacts
{
    public partial class StudentContactService : IStudentContactService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentContactService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<StudentContact> AddStudentContactAsync(StudentContact studentContact) =>
        TryCatch(async () =>
        {
            ValidateStudentContactOnCreate(studentContact);

            return await this.storageBroker.InsertStudentContactAsync(studentContact);
        });

        public ValueTask<StudentContact> RemoveStudentContactByIdAsync(Guid studentId, Guid contactId) =>
        TryCatch(async () =>
        {
            ValidateStudentContactIdIsNull(studentId, contactId);

            StudentContact mayBeStudentContact =
                await this.storageBroker.SelectStudentContactByIdAsync(studentId, contactId);

            ValidateStorageStudentContact(mayBeStudentContact, studentId, contactId);

            return await this.storageBroker.DeleteStudentContactAsync(mayBeStudentContact);
        });

        public IQueryable<StudentContact> RetrieveAllStudentContacts() =>
        TryCatch(() =>
        {
            IQueryable<StudentContact> storageStudentContacts =
                this.storageBroker.SelectAllStudentContacts();

            ValidateStorageStudentContacts(storageStudentContacts);

            return storageStudentContacts;
        });

        public ValueTask<StudentContact> RetrieveStudentContactByIdAsync(Guid studentId, Guid contactId) =>
        TryCatch(async () =>
        {
            ValidateStudentContactIdIsNull(studentId, contactId);

            StudentContact storageStudentContact =
                await this.storageBroker.SelectStudentContactByIdAsync(studentId, contactId);

            ValidateStorageStudentContact(storageStudentContact, studentId, contactId);

            return storageStudentContact;
        });
    }
}