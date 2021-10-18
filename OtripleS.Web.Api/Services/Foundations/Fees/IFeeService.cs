// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Services.Foundations.Fees
{
    public interface IFeeService
    {
        ValueTask<Fee> AddFeeAsync(Fee fee);
        IQueryable<Fee> RetrieveAllFees();
        ValueTask<Fee> RetrieveFeeByIdAsync(Guid feeId);
        ValueTask<Fee> ModifyFeeAsync(Fee fee);
        ValueTask<Fee> RemoveFeeByIdAsync(Guid feeId);
    }
}
