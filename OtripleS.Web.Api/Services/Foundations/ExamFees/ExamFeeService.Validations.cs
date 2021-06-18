//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Foundations.ExamFees;
using OtripleS.Web.Api.Models.Foundations.ExamFees.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.ExamFees
{
    public partial class ExamFeeService
    {
        public void ValidateExamFeeOnCreate(ExamFee examFee)
        {
            ValidateExamFeeIsNull(examFee);
            ValidateExamFeeIds(examFee.ExamId, examFee.FeeId);
            ValidateInvalidAuditFields(examFee);
            ValidateInvalidAuditFieldsOnCreate(examFee);
        }

        private void ValidateExamFeeOnModify(ExamFee examFee)
        {
            ValidateExamFeeIsNull(examFee);
            ValidateExamFeeId(examFee.Id);
            ValidateExamFeeIds(examFee.ExamId, examFee.FeeId);
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
            if (examId == default)
            {
                throw new InvalidExamFeeException(
                    parameterName: nameof(ExamFee.ExamId),
                    parameterValue: examId);
            }
            else if (feeId == default)
            {
                throw new InvalidExamFeeException(
                    parameterName: nameof(ExamFee.FeeId),
                    parameterValue: feeId);
            }
        }

        private static void ValidateInvalidAuditFields(ExamFee examFee)
        {
            switch (examFee)
            {
                case { } when IsInvalid(examFee.CreatedBy):
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.CreatedBy),
                        parameterValue: examFee.CreatedBy);

                case { } when IsInvalid(examFee.CreatedDate):
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.CreatedDate),
                        parameterValue: examFee.CreatedDate);

                case { } when IsInvalid(examFee.UpdatedBy):
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.UpdatedBy),
                        parameterValue: examFee.UpdatedBy);

                case { } when IsInvalid(examFee.UpdatedDate):
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.UpdatedDate),
                        parameterValue: examFee.UpdatedDate);
            }
        }

        private void ValidateInvalidAuditFieldsOnCreate(ExamFee examFee)
        {
            switch (examFee)
            {
                case { } when examFee.UpdatedBy != examFee.CreatedBy:
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.UpdatedBy),
                        parameterValue: examFee.UpdatedBy);

                case { } when examFee.UpdatedDate != examFee.CreatedDate:
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.UpdatedDate),
                        parameterValue: examFee.UpdatedDate);

                case { } when IsDateNotRecent(examFee.CreatedDate):
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.CreatedDate),
                        parameterValue: examFee.CreatedDate);
            }
        }

        private void ValidateStorageExamFees(IQueryable<ExamFee> storageExamFees)
        {
            if (!storageExamFees.Any())
            {
                this.loggingBroker.LogWarning("No exam fees found in storage.");
            }
        }

        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset input) => input == default;

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
