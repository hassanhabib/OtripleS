// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Fees
{
    public partial class FeeService
    {

        private void ValidateFeeOnAdd(Fee fee)
        {
            ValidateFeeIsNotNull(fee);

            Validate(
                (Rule: IsInvalid(fee.Id), Parameter: nameof(Fee.Id)),
                (Rule: IsInvalid(fee.Label), Parameter: nameof(Fee.Label)),
                (Rule: IsInvalid(fee.CreatedBy), Parameter: nameof(Fee.CreatedBy)),
                (Rule: IsInvalid(fee.UpdatedBy), Parameter: nameof(Fee.UpdatedBy)),
                (Rule: IsInvalid(fee.CreatedDate), Parameter: nameof(Fee.CreatedDate)),
                (Rule: IsInvalid(fee.UpdatedDate), Parameter: nameof(Fee.UpdatedDate)),

                (Rule: IsNotSame(
                    firstDate: fee.UpdatedDate,
                    secondDate: fee.CreatedDate,
                    secondDateName: nameof(Fee.CreatedDate)),
                Parameter: nameof(Fee.UpdatedDate)),

                (Rule: IsNotRecent(fee.CreatedDate), Parameter: nameof(Fee.CreatedDate))
            );
        }

        private void ValidateFeeOnModify(Fee fee)
        {
            ValidateFeeIsNotNull(fee);

            Validate(
                (Rule: IsInvalid(fee.Id), Parameter: nameof(Fee.Id)),
                (Rule: IsInvalid(fee.Label), Parameter: nameof(Fee.Label)),
                (Rule: IsInvalid(fee.CreatedBy), Parameter: nameof(Fee.CreatedBy)),
                (Rule: IsInvalid(fee.UpdatedBy), Parameter: nameof(Fee.UpdatedBy)),
                (Rule: IsInvalid(fee.CreatedDate), Parameter: nameof(Fee.CreatedDate)),
                (Rule: IsInvalid(fee.UpdatedDate), Parameter: nameof(Fee.UpdatedDate)));
        }

        private static void ValidateFeeIsNotNull(Fee fee)
        {
            if (fee == default)
            {
                throw new NullFeeException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
        {
            Condition = firstDate != secondDate,
            Message = $"Date is not the same as {secondDateName}"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private static void ValidateFeeId(Guid feeId)
        {
            if (IsInvalidOld(feeId))
            {
                throw new InvalidFeeException(
                    parameterName: nameof(Fee.Id),
                    parameterValue: feeId);
            }
        }

        private static bool IsInvalidOld(Guid input) => input == default;
        private static bool IsInvalidOld(DateTimeOffset input) => input == default;
        private static bool IsInvalidOld(string input) => string.IsNullOrWhiteSpace(input);

        private static void ValidateFeeAuditFields(Fee fee)
        {
            switch (fee)
            {
                case { } when IsInvalidOld(input: fee.CreatedBy):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.CreatedBy),
                        parameterValue: fee.CreatedBy);

                case { } when IsInvalidOld(input: fee.UpdatedBy):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.UpdatedBy),
                        parameterValue: fee.UpdatedBy);

                case { } when IsInvalidOld(input: fee.CreatedDate):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.CreatedDate),
                        parameterValue: fee.CreatedDate);

                case { } when IsInvalidOld(input: fee.UpdatedDate):
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
                case { } when IsInvalidOld(fee.Label):
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidFeeException = new InvalidFeeException();

            foreach((dynamic rule, string parameter) in validations)
            {
                if(rule.Condition)
                {
                    invalidFeeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidFeeException.ThrowIfContainsErrors();
        }
    }
}
