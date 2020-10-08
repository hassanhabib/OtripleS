//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
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

        public IQueryable<StudentContact> RetrieveAllStudentContacts() =>
            TryCatch(() =>
            {
                IQueryable<StudentContact> storageStudentContacts =
                    this.storageBroker.SelectAllStudentContacts();

                ValidateStorageStudentContacts(storageStudentContacts);

                return storageStudentContacts;
            });
    }
}
