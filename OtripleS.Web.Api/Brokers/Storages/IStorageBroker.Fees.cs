// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<Fee> InsertFeeAsync(Fee fee);
        public IQueryable<Fee> SelectAllFees();
        public ValueTask<Fee> SelectFeeByIdAsync(Guid feeId);
        public ValueTask<Fee> UpdateFeeAsync(Fee fee);
        public ValueTask<Fee> DeleteFeeAsync(Fee fee);
    }
}
