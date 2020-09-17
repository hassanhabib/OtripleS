// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Guardians;

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

        public IQueryable<Guardian> RetrieveAllGuardians() =>
        TryCatch(() =>
        {
            IQueryable<Guardian> storageGuardians = this.storageBroker.SelectAllGuardians();
            ValidateStorageGuardians(storageGuardians);

            return storageGuardians;
        });

        public ValueTask<Guardian> CreateGuardianAsync(Guardian guardian) =>
        TryCatch(async () =>
        {
            ValidateGuardianOnCreate(guardian);

            return await this.storageBroker.InsertGuardianAsync(guardian);
        });

        public ValueTask<Guardian> DeleteGuardianByIdAsync(Guid guardianId) =>
        TryCatch(async () =>
        {
            ValidateGuardianId(guardianId);

            Guardian maybeGuardian =
                await this.storageBroker.SelectGuardianByIdAsync(guardianId);

            ValidateStorageGuardian(maybeGuardian, guardianId);

            return await this.storageBroker.DeleteGuardianAsync(maybeGuardian);
        });

        public ValueTask<Guardian> RetrieveGuardianByIdAsync(Guid guardianId) =>
        TryCatch(async () =>
        {
            ValidateGuardianId(guardianId);
            Guardian storageGuardian = await this.storageBroker.SelectGuardianByIdAsync(guardianId);
            ValidateStorageGuardian(storageGuardian, guardianId);

            return storageGuardian;
        });

        public ValueTask<Guardian> ModifyGuardianAsync(Guardian guardian) =>
        TryCatch(async () =>
        {
            ValidateGuardianOnModify(guardian);
            Guardian maybeGuardian = await storageBroker.SelectGuardianByIdAsync(guardian.Id);
            ValidateStorageGuardian(maybeGuardian, guardian.Id);
            ValidateAgainstStorageGuardianOnModify(inputGuardian: guardian, storageGuardian: maybeGuardian);

            return await storageBroker.UpdateGuardianAsync(guardian);
        });
    }
}
