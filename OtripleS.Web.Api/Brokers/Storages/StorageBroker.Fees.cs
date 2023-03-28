// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Fee> Fees { get; set; }

        public async ValueTask<Fee> InsertFeeAsync(Fee fee)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Fee> feeEntityEntry = await broker.Fees.AddAsync(entity: fee);
            await broker.SaveChangesAsync();

            return feeEntityEntry.Entity;
        }

        public IQueryable<Fee> SelectAllFees() => this.Fees;

        public async ValueTask<Fee> SelectFeeByIdAsync(Guid FeeId) =>
            await SelectFeeByIdAsync(FeeId);

        public async ValueTask<Fee> UpdateFeeAsync(Fee fee)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Fee> feeEntityEntry = broker.Fees.Update(entity: fee);
            await broker.SaveChangesAsync();

            return feeEntityEntry.Entity;
        }

        public async ValueTask<Fee> DeleteFeeAsync(Fee fee)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Fee> feeEntityEntry = broker.Fees.Remove(entity: fee);
            await broker.SaveChangesAsync();

            return feeEntityEntry.Entity;
        }
    }
}
