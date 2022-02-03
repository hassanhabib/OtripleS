// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Registrations;

namespace OtripleS.Web.Api.Services.Foundations.Registrations
{
    public partial class RegistrationService : IRegistrationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public RegistrationService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Registration> AddRegistrationAsync(Registration registration) =>
        TryCatch(async () =>
        {
            ValidateRegistrationOnAdd(registration);

            return await this.storageBroker.InsertRegistrationAsync(registration);
        });

        public IQueryable<Registration> RetrieveAllRegistrations() =>
        TryCatch(() => this.storageBroker.SelectAllRegistrations());

        public ValueTask<Registration> RetrieveRegistrationByIdAsync(Guid registrationId) =>
        TryCatch(async () =>
        {
            ValidateRegistrationId(registrationId);

            Registration maybeRegistration =
                await this.storageBroker.SelectRegistrationByIdAsync(registrationId);

            ValidateStorageRegistration(maybeRegistration, registrationId);

            return maybeRegistration;
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
