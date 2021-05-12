// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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

        public async ValueTask<Registration> InsertRegistrationAsync(Registration Registration)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Registration> RegistrationEntityEntry = await broker.Registrations.AddAsync(Registration);
            await broker.SaveChangesAsync();

            return RegistrationEntityEntry.Entity;
        }

        public IQueryable<Registration> SelectAllRegistrations() => Registrations.AsQueryable();

        public async ValueTask<Registration> SelectRegistrationByIdAsync(Guid RegistrationId)
        {
            using var broker = new StorageBroker(this.configuration);
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Registrations.FindAsync(RegistrationId);
        }

        public async ValueTask<Registration> UpdateRegistrationAsync(Registration Registration)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Registration> RegistrationEntityEntry = broker.Registrations.Update(Registration);
            await broker.SaveChangesAsync();

            return RegistrationEntityEntry.Entity;
        }

        public async ValueTask<Registration> DeleteRegistrationAsync(Registration Registration)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Registration> RegistrationEntityEntry = broker.Registrations.Remove(Registration);
            await broker.SaveChangesAsync();

            return RegistrationEntityEntry.Entity;
        }
    }
}
