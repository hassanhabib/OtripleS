// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Services.Foundations.StudentRegistrations
{
    public partial class StudentRegistrationService : IStudentRegistrationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentRegistrationService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }
        
        public ValueTask<StudentRegistration> AddStudentRegistrationAsync(StudentRegistration studentRegistration) =>
        TryCatch(async () =>
        {
            ValidateStudentRegistrationOnCreate(studentRegistration);

            return await storageBroker.InsertStudentRegistrationAsync(studentRegistration);
        });

        public IQueryable<StudentRegistration> RetrieveAllStudentRegistrations() =>
        TryCatch(() => this.storageBroker.SelectAllStudentRegistrations());


        public ValueTask<StudentRegistration> RetrieveStudentRegistrationByIdAsync(
            Guid studentId,
            Guid registrationId) => TryCatch(async () =>
        {
            ValidateStudentRegistrationIds(studentId, registrationId);

            StudentRegistration storageStudentRegistration =
            await this.storageBroker.SelectStudentRegistrationByIdAsync(studentId, registrationId);

            ValidateStorageStudentRegistration(storageStudentRegistration, studentId, registrationId);

            return storageStudentRegistration;
        });

        public ValueTask<StudentRegistration> RemoveStudentRegistrationByIdsAsync(
            Guid studentId,
            Guid registrationId) => TryCatch(async () =>
        {
            ValidateStudentRegistrationIds(studentId, registrationId);

            StudentRegistration maybeStudentRegistration =
                await this.storageBroker.SelectStudentRegistrationByIdAsync(studentId, registrationId);

            ValidateStorageStudentRegistration(maybeStudentRegistration, studentId, registrationId);

            return await this.storageBroker.DeleteStudentRegistrationAsync(maybeStudentRegistration);
        });
    }
}
