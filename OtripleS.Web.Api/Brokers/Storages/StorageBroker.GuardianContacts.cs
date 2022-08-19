// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.GuardianContacts;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<GuardianContact> GuardianContacts { get; set; }

        public async ValueTask<GuardianContact> InsertGuardianContactAsync(
            GuardianContact guardianContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<GuardianContact> guardianContactEntityEntry =
                await broker.GuardianContacts.AddAsync(entity: guardianContact);

            await broker.SaveChangesAsync();

            return guardianContactEntityEntry.Entity;
        }

        public IQueryable<GuardianContact> SelectAllGuardianContacts() =>
            this.GuardianContacts;

        public async ValueTask<GuardianContact> SelectGuardianContactByIdAsync(
            Guid guardianId,
            Guid contactId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.GuardianContacts.FindAsync(guardianId, contactId);
        }

        public async ValueTask<GuardianContact> UpdateGuardianContactAsync(
            GuardianContact guardianContact)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<GuardianContact> guardianContactEntityEntry =
                broker.GuardianContacts.Update(entity: guardianContact);

            await broker.SaveChangesAsync();

            return guardianContactEntityEntry.Entity;
        }

        public async ValueTask<GuardianContact> DeleteGuardianContactAsync(
            GuardianContact guardianContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<GuardianContact> guardianContactEntityEntry =
                broker.GuardianContacts.Remove(entity: guardianContact);

            await broker.SaveChangesAsync();

            return guardianContactEntityEntry.Entity;
        }
    }
}
