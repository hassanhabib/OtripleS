// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;

namespace OtripleS.Web.Api.Services.Fees
{
    public partial class FeeService
    {
        private void ValidateStorageFees(IQueryable<Fee> storageFees)
        {
            if (storageFees.Count() == 0)
            {
                this.loggingBroker.LogWarning("No fees found in storage.");
            }
        }

        private void ValidateFeeId(Guid feeId)
        {
            if (feeId == Guid.Empty)
            {
                throw new InvalidFeeInputException(
                    parameterName: nameof(Fee.Id),
                    parameterValue: feeId);
            }
        }


        private void ValidateStorageFee(Fee storageFee, Guid feeId)
        {
            if (storageFee == null)
            {
                throw new NotFoundFeeException(feeId);
            }
        }
    }
}
