// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Fees;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Fee> Fees { get; set; }

        public async ValueTask<Fee> InsertFeeAsync(Fee fee)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Fee> feeEntityEntry = await broker.Fees.AddAsync(fee);
            await broker.SaveChangesAsync();

            return feeEntityEntry.Entity;
        }

        public IQueryable<Fee> SelectAllFees() => Fees.AsQueryable();

        public async ValueTask<Fee> SelectFeeByIdAsync(Guid feeId)
        {
            using var broker = new StorageBroker(this.configuration);
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Fees.FindAsync(feeId);
        }

        public async ValueTask<Fee> UpdateFeeAsync(Fee fee)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Fee> feeEntityEntry = broker.Fees.Update(fee);
            await broker.SaveChangesAsync();

            return feeEntityEntry.Entity;
        }

        public async ValueTask<Fee> DeleteFeeAsync(Fee fee)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Fee> feeEntityEntry = broker.Fees.Remove(fee);
            await broker.SaveChangesAsync();

            return feeEntityEntry.Entity;
        }
    }
}
