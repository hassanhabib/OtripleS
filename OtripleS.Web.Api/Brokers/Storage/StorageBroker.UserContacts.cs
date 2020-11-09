// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.UserContacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<UserContact> UserContacts { get; set; }

        public async ValueTask<UserContact> InsertUserContactAsync(
            UserContact UserContact)
        {
            EntityEntry<UserContact> UserContactEntityEntry =
                await this.UserContacts.AddAsync(UserContact);

            await this.SaveChangesAsync();

            return UserContactEntityEntry.Entity;
        }

        public IQueryable<UserContact> SelectAllUserContacts() =>
            this.UserContacts.AsQueryable();

        public async ValueTask<UserContact> SelectUserContactByIdAsync(
            Guid userId,
            Guid contactId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.UserContacts.FindAsync(userId, contactId);
        }

        public async ValueTask<UserContact> UpdateUserContactAsync(
            UserContact UserContact)
        {
            EntityEntry<UserContact> UserContactEntityEntry =
                this.UserContacts.Update(UserContact);

            await this.SaveChangesAsync();

            return UserContactEntityEntry.Entity;
        }

        public async ValueTask<UserContact> DeleteUserContactAsync(
            UserContact UserContact)
        {
            EntityEntry<UserContact> UserContactEntityEntry =
                this.UserContacts.Remove(UserContact);

            await this.SaveChangesAsync();

            return UserContactEntityEntry.Entity;
        }
    }
}
