//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.ExamFees;

namespace OtripleS.Web.Api.Services.Foundations.ExamFees
{
    public partial class ExamFeeService : IExamFeeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public ExamFeeService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<ExamFee> AddExamFeeAsync(ExamFee examFee) =>
        TryCatch(async () =>
        {
            ValidateExamFeeOnCreate(examFee);

            return await this.storageBroker.InsertExamFeeAsync(examFee);
        });

        public IQueryable<ExamFee> RetrieveAllExamFees() =>
        TryCatch(() =>
        {
            IQueryable<ExamFee> storageExamFees = this.storageBroker.SelectAllExamFees();

            ValidateStorageExamFees(storageExamFees);

            return storageExamFees;

        });

        public ValueTask<ExamFee> RetrieveExamFeeByIdAsync(Guid examFeeId) =>
        TryCatch(async () =>
        {
            ValidateExamFeeId(examFeeId);

            ExamFee maybeExamFee =
                await this.storageBroker.SelectExamFeeByIdAsync(examFeeId);

            ValidateStorageExamFee(maybeExamFee, examFeeId);

            return maybeExamFee;
        });

        public ValueTask<ExamFee> ModifyExamFeeAsync(ExamFee examFee) =>
        TryCatch(async () =>
        {
            ValidateExamFeeOnModify(examFee);

            ExamFee maybeExamFee =
                await this.storageBroker.SelectExamFeeByIdAsync(examFee.Id);

            ValidateStorageExamFee(maybeExamFee, examFee.Id);
            ValidateAgainstStorageExamFeeOnModify(inputExamFee: examFee, storageExamFee: maybeExamFee);

            return await this.storageBroker.UpdateExamFeeAsync(examFee);
        });

        public ValueTask<ExamFee> RemoveExamFeeByIdAsync(Guid examFeeId) =>
        TryCatch(async () =>
        {
            ValidateExamFeeId(examFeeId);

            ExamFee maybeExamFee =
                await this.storageBroker.SelectExamFeeByIdAsync(examFeeId);

            ValidateStorageExamFee(maybeExamFee, examFeeId);

            return await this.storageBroker.DeleteExamFeeAsync(maybeExamFee);
        });
    }
}
