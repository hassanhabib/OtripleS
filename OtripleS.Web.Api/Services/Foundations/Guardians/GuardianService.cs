// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Guardians;

namespace OtripleS.Web.Api.Services.Foundations.Guardians
{
    public partial class GuardianService : IGuardianService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public GuardianService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Guardian> CreateGuardianAsync(Guardian guardian) =>
        TryCatch(async () =>
        {
            ValidateGuardianOnCreate(guardian);

            return await this.storageBroker.InsertGuardianAsync(guardian);
        });

        public IQueryable<Guardian> RetrieveAllGuardians() =>
        TryCatch(() => this.storageBroker.SelectAllGuardians());

        public ValueTask<Guardian> RetrieveGuardianByIdAsync(Guid guardianId) =>
        TryCatch(async () =>
        {
            ValidateGuardianId(guardianId);
            Guardian maybeGuardian = await this.storageBroker.SelectGuardianByIdAsync(guardianId);
            ValidateStorageGuardian(maybeGuardian, guardianId);

            return maybeGuardian;
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

        public ValueTask<Guardian> RemoveGuardianByIdAsync(Guid guardianId) =>
        TryCatch(async () =>
        {
            ValidateGuardianId(guardianId);

            Guardian maybeGuardian =
                await this.storageBroker.SelectGuardianByIdAsync(guardianId);

            ValidateStorageGuardian(maybeGuardian, guardianId);

            return await this.storageBroker.DeleteGuardianAsync(maybeGuardian);
        });
    }
}
