//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Services.StudentRegistrations
{
    public partial class StudentRegistrationService : IStudentRegistrationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public StudentRegistrationService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<StudentRegistration> RetrieveStudentRegistrationByIdAsync(Guid studentId, Guid registrationId) =>
        TryCatch(async () =>
        {
            ValidateStudentRegistrationId(studentId, registrationId);

            StudentRegistration storageStudentRegistration =
            await this.storageBroker.SelectStudentRegistrationByIdAsync(studentId, registrationId);

            ValidateStorageStudentRegistration(storageStudentRegistration, studentId, registrationId);

            return storageStudentRegistration;
        });
    }
}
