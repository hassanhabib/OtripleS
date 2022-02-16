// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.UserContacts;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<UserContact> UserContacts { get; set; }

        public async ValueTask<UserContact> InsertUserContactAsync(
            UserContact userContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<UserContact> userContactEntityEntry =
                await broker.UserContacts.AddAsync(entity: userContact);

            await broker.SaveChangesAsync();

            return userContactEntityEntry.Entity;
        }

        public IQueryable<UserContact> SelectAllUserContacts() => this.UserContacts;

        public async ValueTask<UserContact> SelectUserContactByIdAsync(
            Guid userId,
            Guid contactId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.UserContacts.FindAsync(userId, contactId);
        }

        public async ValueTask<UserContact> UpdateUserContactAsync(
            UserContact userContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<UserContact> userContactEntityEntry =
                broker.UserContacts.Update(entity: userContact);

            await broker.SaveChangesAsync();

            return userContactEntityEntry.Entity;
        }

        public async ValueTask<UserContact> DeleteUserContactAsync(
            UserContact userContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<UserContact> userContactEntityEntry =
                broker.UserContacts.Remove(entity: userContact);

            await broker.SaveChangesAsync();

            return userContactEntityEntry.Entity;
        }
    }
}
