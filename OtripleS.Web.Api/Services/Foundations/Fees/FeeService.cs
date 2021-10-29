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
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Services.Foundations.Fees
{
    public partial class FeeService : IFeeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public FeeService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Fee> AddFeeAsync(Fee fee) =>
        TryCatch(async () =>
        {
            ValidateFeeOnAdd(fee);

            return await this.storageBroker.InsertFeeAsync(fee);
        });

        public IQueryable<Fee> RetrieveAllFees() =>
        TryCatch(() =>
        {
            IQueryable<Fee> storageFees = this.storageBroker.SelectAllFees();

            ValidateStorageFees(storageFees);

            return storageFees;
        });

        public ValueTask<Fee> RetrieveFeeByIdAsync(Guid feeId) =>
        TryCatch(async () =>
        {
            ValidateFeeId(feeId);

            Fee maybeFee =
                await this.storageBroker.SelectFeeByIdAsync(feeId);

            ValidateStorageFee(maybeFee, feeId);

            return maybeFee;
        });

        public ValueTask<Fee> ModifyFeeAsync(Fee fee) =>
        TryCatch(async () =>
        {
            ValidateFeeOnModify(fee);
            Fee maybeFee = await this.storageBroker.SelectFeeByIdAsync(fee.Id);
            ValidateStorageFee(maybeFee, fee.Id);
            ValidateAgainstStorageFeeOnModify(inputFee: fee, storageFee: maybeFee);

            return await this.storageBroker.UpdateFeeAsync(fee);
        });

        public ValueTask<Fee> RemoveFeeByIdAsync(Guid feeId) =>
        TryCatch(async () =>
        {
            ValidateFeeId(feeId);

            Fee maybeFee =
               await this.storageBroker.SelectFeeByIdAsync(feeId);

            ValidateStorageFee(maybeFee, feeId);

            return await this.storageBroker.DeleteFeeAsync(maybeFee);
        });
    }
}
