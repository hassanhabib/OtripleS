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
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Services.Foundations.Fees
{
    public partial class FeeService : IFeeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public FeeService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Fee> AddFeeAsync(Fee fee) =>
        TryCatch(async () =>
        {
            ValidateFeeOnAdd(fee);

            return await this.storageBroker.InsertFeeAsync(fee);
        });

        public IQueryable<Fee> RetrieveAllFees() =>
        TryCatch(() => this.storageBroker.SelectAllFees());

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
