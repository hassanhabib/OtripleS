// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Registrations;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Registration> Registrations { get; set; }

        public async ValueTask<Registration> InsertRegistrationAsync(Registration registration)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Registration> registrationEntityEntry = await broker.Registrations.AddAsync(entity: registration);
            await broker.SaveChangesAsync();

            return registrationEntityEntry.Entity;
        }

        public IQueryable<Registration> SelectAllRegistrations() => this.Registrations;

        public async ValueTask<Registration> SelectRegistrationByIdAsync(Guid registrationId)
        {
            using var broker = new StorageBroker(this.configuration);
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Registrations.FindAsync(registrationId);
        }

        public async ValueTask<Registration> UpdateRegistrationAsync(Registration registration)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Registration> registrationEntityEntry = broker.Registrations.Update(entity: registration);
            await broker.SaveChangesAsync();

            return registrationEntityEntry.Entity;
        }

        public async ValueTask<Registration> DeleteRegistrationAsync(Registration registration)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Registration> registrationEntityEntry = broker.Registrations.Remove(entity: registration);
            await broker.SaveChangesAsync();

            return registrationEntityEntry.Entity;
        }
    }
}
