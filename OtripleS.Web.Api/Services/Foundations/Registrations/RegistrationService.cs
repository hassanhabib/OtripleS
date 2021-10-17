// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Registrations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.Registrations
{
    public partial class RegistrationService : IRegistrationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public RegistrationService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Registration> AddRegistrationAsync(Registration registration) =>
        TryCatch(async () =>
        {
            ValidateRegistrationOnAdd(registration);

            return await this.storageBroker.InsertRegistrationAsync(registration);
        });

        public IQueryable<Registration> RetrieveAllRegistrations() =>
        TryCatch(() =>
        {
            IQueryable<Registration> storageRegistrations = this.storageBroker.SelectAllRegistrations();

            ValidateStorageRegistrations(storageRegistrations);

            return storageRegistrations;
        });

        public ValueTask<Registration> RetrieveRegistrationByIdAsync(Guid registrationId) =>
        TryCatch(async () =>
        {
            ValidateRegistrationId(registrationId);

            Registration storageRegistration =
                await this.storageBroker.SelectRegistrationByIdAsync(registrationId);

            ValidateStorageRegistration(storageRegistration, registrationId);

            return storageRegistration;
        });

        public ValueTask<Registration> ModifyRegistrationAsync(Registration registration) =>
        TryCatch(async () =>
        {
            ValidateRegistrationOnModify(registration);
            Registration maybeRegistration = await storageBroker.SelectRegistrationByIdAsync(registration.Id);
            ValidateStorageRegistration(maybeRegistration, registration.Id);

            ValidateAgainstStorageRegistrationOnModify(
                inputRegistration: registration,
                storageRegistration: maybeRegistration);

            return await this.storageBroker.UpdateRegistrationAsync(registration);
        });

        public ValueTask<Registration> RemoveRegistrationByIdAsync(Guid registrationId) =>
        TryCatch(async () =>
        {
            ValidateRegistrationId(registrationId);

            Registration maybeRegistration =
               await this.storageBroker.SelectRegistrationByIdAsync(registrationId);

            ValidateStorageRegistration(maybeRegistration, registrationId);

            return await this.storageBroker.DeleteRegistrationAsync(maybeRegistration);
        });
    }
}
