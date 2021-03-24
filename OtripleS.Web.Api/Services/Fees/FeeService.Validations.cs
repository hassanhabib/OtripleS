// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;

namespace OtripleS.Web.Api.Services.Fees
{
    public partial class FeeService
    {
        private void ValidateFeeOnAdd(Fee fee)
        {
            ValidateFeeIsNotNull(fee);
        }

        private void ValidateFeeIsNotNull(Fee fee)
        {
            if (fee == default)
            {
                throw new NullFeeException();
            }
        }
    }
}
