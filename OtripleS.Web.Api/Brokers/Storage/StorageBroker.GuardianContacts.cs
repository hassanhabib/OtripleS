// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.GuardianContacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<GuardianContact> GuardianContacts { get; set; }

        public async ValueTask<GuardianContact> InsertGuardianContactAsync(
            GuardianContact GuardianContact)
        {
            EntityEntry<GuardianContact> GuardianContactEntityEntry =
                await this.GuardianContacts.AddAsync(GuardianContact);

            await this.SaveChangesAsync();

            return GuardianContactEntityEntry.Entity;
        }

        public IQueryable<GuardianContact> SelectAllGuardianContacts() =>
            this.GuardianContacts.AsQueryable();

        public async ValueTask<GuardianContact> SelectGuardianContactByIdAsync(
            Guid guardianId,
            Guid contactId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.GuardianContacts.FindAsync(guardianId, contactId);
        }

        public async ValueTask<GuardianContact> UpdateGuardianContactAsync(
            GuardianContact GuardianContact)
        {
            EntityEntry<GuardianContact> GuardianContactEntityEntry =
                this.GuardianContacts.Update(GuardianContact);

            await this.SaveChangesAsync();

            return GuardianContactEntityEntry.Entity;
        }

        public async ValueTask<GuardianContact> DeleteGuardianContactAsync(
            GuardianContact GuardianContact)
        {
            EntityEntry<GuardianContact> GuardianContactEntityEntry =
                this.GuardianContacts.Remove(GuardianContact);

            await this.SaveChangesAsync();

            return GuardianContactEntityEntry.Entity;
        }
    }
}
