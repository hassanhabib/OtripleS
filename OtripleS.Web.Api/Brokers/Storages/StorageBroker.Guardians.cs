// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Guardians;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guardian> Guardians { get; set; }

        public async ValueTask<Guardian> InsertGuardianAsync(Guardian guardian)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Guardian> guardianEntityEntry = await broker.Guardians.AddAsync(entity: guardian);
            await broker.SaveChangesAsync();

            return guardianEntityEntry.Entity;
        }

        public IQueryable<Guardian> SelectAllGuardians() => this.Guardians;

        public async ValueTask<Guardian> SelectGuardianByIdAsync(Guid guardianId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Guardians.FindAsync(guardianId);
        }

        public async ValueTask<Guardian> UpdateGuardianAsync(Guardian guardian)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Guardian> courseEntityEntry = broker.Guardians.Update(entity: guardian);
            await broker.SaveChangesAsync();

            return courseEntityEntry.Entity;
        }

        public async ValueTask<Guardian> DeleteGuardianAsync(Guardian guardian)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Guardian> courseEntityEntry = broker.Guardians.Remove(entity: guardian);
            await broker.SaveChangesAsync();

            return courseEntityEntry.Entity;
        }
    }
}
