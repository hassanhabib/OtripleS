// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;

namespace OtripleS.Web.Api.Services.Fees
{
    public partial class FeeService
    {
        private void ValidateFeeOnAdd(Fee fee)
        {
            ValidateFeeIsNotNull(fee);
            ValidateFeeId(fee.Id);
            ValidateFeeAuditFieldsOnCreate(fee);
        }

        private void ValidateFeeIsNotNull(Fee fee)
        {
            if (fee == default)
            {
                throw new NullFeeException();
            }
        }

        private void ValidateFeeId(Guid feeId)
        {
            if (IsInvalid(feeId))
            {
                throw new InvalidFeeException(
                    parameterName: nameof(Fee.Id),
                    parameterValue: feeId);
            }
        }

        private bool IsInvalid(Guid input) => input == default;

        private void ValidateFeeAuditFieldsOnCreate(Fee fee)
        {
            switch (fee)
            {
                case { } when IsInvalid(input: fee.CreatedBy):
                    throw new InvalidFeeException(
                        parameterName: nameof(Fee.CreatedBy),
                        parameterValue: fee.CreatedBy);
            }
        }
    }
}
