// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Guardian;

namespace OtripleS.Web.Api.Services.Guardians
{
    public partial class GuardianService : IGuardianService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public GuardianService(IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Guardian> CreateGuardianAsync(Guardian guardian) =>
        TryCatch(async () =>
        { 
            ValidateGuardian(guardian);

            return await this.storageBroker.InsertGuardianAsync(guardian);
        });

        public async ValueTask<Guardian> DeleteGuardianAsync(Guid guardianId)
        {
            Guardian maybeGuardian =
                await this.storageBroker.SelectGuardianByIdAsync(guardianId);

            return await this.storageBroker.DeleteGuardianAsync(maybeGuardian);
        }

        public ValueTask<Guardian> RetrieveGuardianByIdAsync(Guid guardianId) =>
        TryCatch(async () =>
        {
            ValidateGuardianId(guardianId);
            Guardian storageGuardian = await this.storageBroker.SelectGuardianByIdAsync(guardianId);
            ValidateStorageGuardian(storageGuardian, guardianId);

            return storageGuardian;
        });
    }
}
