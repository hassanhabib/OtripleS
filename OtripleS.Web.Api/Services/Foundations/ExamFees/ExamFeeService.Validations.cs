//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.ExamFees
{
    public partial class ExamFeeService
    {
        public void ValidateExamFeeOnCreate(ExamFee examFee)
        {
            ValidateExamFeeIsNull(examFee);

            Validate(
                (Rule: IsInvalid(examFee.ExamId), Parameter: nameof(ExamFee.ExamId)),
                (Rule: IsInvalid(examFee.FeeId), Parameter: nameof(ExamFee.FeeId)),
                (Rule: IsInvalid(examFee.CreatedBy), Parameter: nameof(ExamFee.CreatedBy)),
                (Rule: IsInvalid(examFee.CreatedDate), Parameter: nameof(ExamFee.CreatedDate)),
                (Rule: IsInvalid(examFee.UpdatedBy), Parameter: nameof(ExamFee.UpdatedBy)),
                (Rule: IsInvalid(examFee.UpdatedDate), Parameter: nameof(ExamFee.UpdatedDate)),
                (Rule: IsNotRecent(examFee.CreatedDate), Parameter: nameof(ExamFee.CreatedDate)),

                (Rule: IsNotSame(
                    firstId: examFee.UpdatedBy,
                    secondId: examFee.CreatedBy,
                    secondIdName: nameof(ExamFee.CreatedBy)),
                Parameter: nameof(ExamFee.UpdatedBy)),
                (Rule: IsNotSame(
                    firstId: examFee.UpdatedDate,
                    secondId: examFee.CreatedDate,
                    secondIdName: nameof(ExamFee.CreatedDate)),
                Parameter: nameof(ExamFee.UpdatedDate)));
        }

        private void ValidateExamFeeOnModify(ExamFee examFee)
        {
            ValidateExamFeeIsNull(examFee);

            Validate(
                (Rule: IsInvalid(examFee.Id), Parameter: nameof(ExamFee.Id)),
                (Rule: IsInvalid(examFee.ExamId), Parameter: nameof(ExamFee.ExamId)),
                (Rule: IsInvalid(examFee.FeeId), Parameter: nameof(ExamFee.FeeId)),
                (Rule: IsInvalid(examFee.CreatedBy), Parameter: nameof(ExamFee.CreatedBy)),
                (Rule: IsInvalid(examFee.UpdatedBy), Parameter: nameof(ExamFee.UpdatedBy)));

            ValidateInvalidAuditFields(examFee);
            ValidateInvalidAuditFieldsOnModify(examFee);
        }

        private void ValidateInvalidAuditFieldsOnModify(ExamFee examFee)
        {
            switch (examFee)
            {
                case { } when examFee.UpdatedDate == examFee.CreatedDate:
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.UpdatedDate),
                        parameterValue: examFee.UpdatedDate);

                case { } when IsDateNotRecent(examFee.UpdatedDate):
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.UpdatedDate),
                        parameterValue: examFee.UpdatedDate);
            }
        }

        private static void ValidateAgainstStorageExamFeeOnModify(ExamFee inputExamFee, ExamFee storageExamFee)
        {
            switch (inputExamFee)
            {
                case { } when inputExamFee.CreatedDate != storageExamFee.CreatedDate:
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.CreatedDate),
                        parameterValue: inputExamFee.CreatedDate);

                case { } when inputExamFee.CreatedBy != storageExamFee.CreatedBy:
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.CreatedBy),
                        parameterValue: inputExamFee.CreatedBy);

                case { } when inputExamFee.UpdatedDate == storageExamFee.UpdatedDate:
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.UpdatedDate),
                        parameterValue: inputExamFee.UpdatedDate);
            }
        }

        private static void ValidateExamFeeIsNull(ExamFee examFee)
        {
            if (examFee is null)
            {
                throw new NullExamFeeException();
            }
        }

        private static void ValidateExamFeeIds(Guid examId, Guid feeId)
        {
            Validate(
                (Rule: IsInvalid(examId), Parameter: nameof(ExamFee.ExamId)),
                (Rule: IsInvalid(feeId), Parameter: nameof(ExamFee.FeeId)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstId,
            DateTimeOffset secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Date is not the same as {secondIdName}"
            };

        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidExamFeeException = new InvalidExamFeeException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidExamFeeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidExamFeeException.ThrowIfContainsErrors();
        }

        private void ValidateInvalidAuditFields(ExamFee examFee)
        {
            Validate(
                (Rule: IsInvalid(examFee.CreatedBy), Parameter: nameof(ExamFee.CreatedBy)),
                (Rule: IsInvalid(examFee.CreatedDate), Parameter: nameof(ExamFee.CreatedDate)),
                (Rule: IsInvalid(examFee.UpdatedBy), Parameter: nameof(ExamFee.UpdatedBy)),
                (Rule: IsInvalid(examFee.UpdatedDate), Parameter: nameof(ExamFee.UpdatedDate)),
                (Rule: IsNotRecent(examFee.CreatedDate), Parameter: nameof(ExamFee.CreatedDate)));
        }

        private void ValidateStorageExamFees(IQueryable<ExamFee> storageExamFees)
        {
            if (!storageExamFees.Any())
            {
                this.loggingBroker.LogWarning("No exam fees found in storage.");
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private static void ValidateExamFeeId(Guid examFeeId)
        {
            if (examFeeId == Guid.Empty)
            {
                throw new InvalidExamFeeException(
                    parameterName: nameof(ExamFee.Id),
                    parameterValue: examFeeId);
            }
        }

        private static void ValidateStorageExamFee(ExamFee storageExamFee, Guid examFeeId)
        {
            if (storageExamFee == null)
            {
                throw new NotFoundExamFeeException(examFeeId);
            }
        }
    }
}
