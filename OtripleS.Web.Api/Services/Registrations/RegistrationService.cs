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

namespace OtripleS.Web.Api.Services.Registrations
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

        public async ValueTask<Registration> AddRegistrationAsync(Registration registration)
        {
            return await this.storageBroker.InsertRegistrationAsync(registration);
        }

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

    }
}
