// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Fee> Fees { get; set; }

        public async ValueTask<Fee> InsertFeeAsync(Fee fee)
        {
            var feeEntityEntry = await this.Fees.AddAsync(fee);
            await this.SaveChangesAsync();

            return feeEntityEntry.Entity;
        }

        public IQueryable<Fee> SelectAllFees() => this.Fees.AsQueryable();

        public async ValueTask<Fee> SelectFeeByIdAsync(Guid feeId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.Fees.FindAsync(feeId);
        }

        public async ValueTask<Fee> UpdateFeeAsync(Fee fee)
        {
            var feeEntityEntry = this.Fees.Update(fee);
            await this.SaveChangesAsync();

            return feeEntityEntry.Entity;
        }

        public async ValueTask<Fee> DeleteFeeAsync(Fee fee)
        {
            var feeEntityEntry = this.Fees.Remove(fee);
            await this.SaveChangesAsync();

            return feeEntityEntry.Entity;
        }
    }
}
