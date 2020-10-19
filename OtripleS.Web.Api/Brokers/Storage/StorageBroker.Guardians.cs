// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Guardians;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Guardian> Guardians { get; set; }

        public async ValueTask<Guardian> InsertGuardianAsync(Guardian guardian)
        {
            EntityEntry<Guardian> guardianEntityEntry = await this.Guardians.AddAsync(guardian);
            await this.SaveChangesAsync();

            return guardianEntityEntry.Entity;
        }

        public IQueryable<Guardian> SelectAllGuardians() => this.Guardians.AsQueryable();

        public async ValueTask<Guardian> SelectGuardianByIdAsync(Guid guardianId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Guardians.FindAsync(guardianId);
        }

        public async ValueTask<Guardian> UpdateGuardianAsync(Guardian guardian)
        {
            EntityEntry<Guardian> courseEntityEntry = this.Guardians.Update(guardian);
            await this.SaveChangesAsync();

            return courseEntityEntry.Entity;
        }

        public async ValueTask<Guardian> DeleteGuardianAsync(Guardian guardian)
        {
            EntityEntry<Guardian> courseEntityEntry = this.Guardians.Remove(guardian);
            await this.SaveChangesAsync();

            return courseEntityEntry.Entity;
        }
    }
}
