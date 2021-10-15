// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;
using System;
using System.Linq;

namespace OtripleS.Web.Api.Services.Foundations.Fees
{
    public partial class FeeService
    {

        private void ValidateFeeOnAdd(Fee fee)
        {
            ValidateFeeIsNotNull(fee);
            ValidateFeeId(fee.Id);
            ValidateFeeProperties(fee);
            ValidateFeeAuditFields(fee);
            ValidateFeeAuditFieldsOnCreate(fee);
        }


        private void ValidateFeeOnModify(Fee fee)
        {
            ValidateFeeIsNotNull(fee);
            ValidateFeeId(fee.Id);
            ValidateFeeProperties(fee);
            ValidateFeeAuditFields(fee);
            ValidateFeeAuditFieldsOnModify(fee);
        }

        private static void ValidateFeeIsNotNull(Fee fee)
        {
            if (fee == default)
            {
                throw new NullFeeException();
            }
        }

        private static void ValidateFeeId(Guid feeId)
        {
            if (IsInvalid(feeId))
            {
                throw new InvalidFeeException(
                    parameterName: nameof(Fee.Id),
                    parameterValue: feeId);
            }
        }

        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset input) => input == default;
        private static bool IsInvalid(string input) => string.IsNullOrWhiteSpace(input);

        private void ValidateFeeAuditFieldsOnCreate(Fee fee)
        {
            switch (fee)
            {
                case { } when fee.UpdatedDate != fee.CreatedDate:
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.UpdatedDate),
                        parameterValue: fee.UpdatedDate);

                case { } when IsDateNotRecent(fee.CreatedDate):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.CreatedDate),
                        parameterValue: fee.CreatedDate);
            }
        }

        private static void ValidateFeeAuditFields(Fee fee)
        {
            switch (fee)
            {
                case { } when IsInvalid(input: fee.CreatedBy):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.CreatedBy),
                        parameterValue: fee.CreatedBy);

                case { } when IsInvalid(input: fee.UpdatedBy):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.UpdatedBy),
                        parameterValue: fee.UpdatedBy);

                case { } when IsInvalid(input: fee.CreatedDate):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.CreatedDate),
                        parameterValue: fee.CreatedDate);

                case { } when IsInvalid(input: fee.UpdatedDate):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.UpdatedDate),
                        parameterValue: fee.UpdatedDate);
            }
        }

        private void ValidateFeeAuditFieldsOnModify(Fee fee)
        {
            switch (fee)
            {
                case { } when fee.UpdatedDate == fee.CreatedDate:
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.UpdatedDate),
                        parameterValue: fee.UpdatedDate);

                case { } when IsDateNotRecent(fee.UpdatedDate):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.UpdatedDate),
                        parameterValue: fee.UpdatedDate);
            }
        }

        private static void ValidateFeeProperties(Fee fee)
        {
            switch (fee)
            {
                case { } when IsInvalid(fee.Label):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.Label),
                        parameterValue: fee.Label);
            }
        }

        private static void ValidateAgainstStorageFeeOnModify(Fee inputFee, Fee storageFee)
        {
            switch (inputFee)
            {
                case { } when inputFee.CreatedDate != storageFee.CreatedDate:
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.CreatedDate),
                        parameterValue: inputFee.CreatedDate);

                case { } when inputFee.CreatedBy != storageFee.CreatedBy:
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.CreatedBy),
                        parameterValue: inputFee.CreatedBy);

                case { } when inputFee.UpdatedDate == storageFee.UpdatedDate:
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.UpdatedDate),
                        parameterValue: inputFee.UpdatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private void ValidateStorageFees(IQueryable<Fee> storageFees)
        {
            if (!storageFees.Any())
            {
                this.loggingBroker.LogWarning("No fees found in storage.");
            }
        }

        private static void ValidateStorageFee(Fee storageFee, Guid feeId)
        {
            if (storageFee == null)
            {
                throw new NotFoundFeeException(feeId);
            }
        }
    }
}
