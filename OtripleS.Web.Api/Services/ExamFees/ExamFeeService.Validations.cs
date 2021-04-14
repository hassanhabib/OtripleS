//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;

namespace OtripleS.Web.Api.Services.ExamFees
{
    public partial class ExamFeeService
    {
        public void ValidateExamFeeOnCreate(ExamFee examFee)
        {
            ValidateExamFeeIsNull(examFee);
            ValidateExamFeeIds(examFee.ExamId, examFee.FeeId);
            ValidateInvalidAuditFields(examFee);
        }

        private void ValidateExamFeeIsNull(ExamFee examFee)
        {
            if (examFee is null)
            {
                throw new NullExamFeeException();
            }
        }

        private void ValidateExamFeeIds(Guid examId, Guid feeId)
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

        private void ValidateInvalidAuditFields(ExamFee examFee)
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

                case { } when examFee.UpdatedBy != examFee.CreatedBy:
                    throw new InvalidExamFeeException(
                        parameterName: nameof(ExamFee.UpdatedBy),
                        parameterValue: examFee.UpdatedBy);
            }
        }

        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset input) => input == default;
    }
}
