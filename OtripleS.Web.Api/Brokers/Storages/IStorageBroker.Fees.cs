// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Fees;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Fee> InsertFeeAsync(Fee fee);
        IQueryable<Fee> SelectAllFees();
        ValueTask<Fee> SelectFeeByIdAsync(Guid feeId);
        ValueTask<Fee> UpdateFeeAsync(Fee fee);
        ValueTask<Fee> DeleteFeeAsync(Fee fee);
    }
}