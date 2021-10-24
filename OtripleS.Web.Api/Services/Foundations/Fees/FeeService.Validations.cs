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

        private static void ValidateFeeId(Guid id) =>
            Validate((Rule: IsInvalid(id), Parameter: nameof(Fee.Id)));

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

                (Rule: IsNotRecent(fee.CreatedDate), Parameter: nameof(Fee.CreatedDate)));
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
                (Rule: IsInvalid(fee.UpdatedDate), Parameter: nameof(Fee.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: fee.UpdatedDate,
                    secondDate: fee.CreatedDate,
                    secondDateName: nameof(Fee.CreatedDate)),
                Parameter: nameof(Fee.UpdatedDate)),

                (Rule: IsNotRecent(fee.UpdatedDate), Parameter: nameof(Fee.UpdatedDate)));
        }

        private static void ValidateAgainstStorageFeeOnModify(Fee inputFee, Fee storageFee)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputFee.CreatedDate,
                    secondDate: storageFee.CreatedDate,
                    secondDateName: nameof(Fee.CreatedDate)),
                Parameter: nameof(Fee.CreatedDate)),

                (Rule: IsNotSame(
                    firstId: inputFee.CreatedBy,
                    secondId: storageFee.CreatedBy,
                    secondIdName: nameof(Fee.CreatedBy)),
                Parameter: nameof(Fee.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputFee.UpdatedDate,
                    secondDate: storageFee.UpdatedDate,
                    secondDateName: nameof(Fee.UpdatedDate)),
                Parameter: nameof(Fee.UpdatedDate)));
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

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
        {
            Condition = firstId != secondId,
            Message = $"Id is not the same as {secondIdName}"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
        {
            Condition = firstDate == secondDate,
            Message = $"Date is the same as {secondDateName}"
        };

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
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
