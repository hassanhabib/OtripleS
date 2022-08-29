// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Contacts;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Contact> Contacts { get; set; }

        public async ValueTask<Contact> InsertContactAsync(Contact contact)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Contact> contactEntityEntry = await broker.Contacts.AddAsync(entity: contact);
            await broker.SaveChangesAsync();

            return contactEntityEntry.Entity;
        }

        public IQueryable<Contact> SelectAllContacts() => this.Contacts;

        public async ValueTask<Contact> SelectContactByIdAsync(Guid contactId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Contacts.FindAsync(contactId);
        }

        public async ValueTask<Contact> UpdateContactAsync(Contact contact)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Contact> contactEntityEntry = broker.Contacts.Update(entity: contact);
            await broker.SaveChangesAsync();

            return contactEntityEntry.Entity;
        }

        public async ValueTask<Contact> DeleteContactAsync(Contact contact)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Contact> contactEntityEntry = broker.Contacts.Remove(entity: contact);
            await broker.SaveChangesAsync();

            return contactEntityEntry.Entity;
        }
    }
}
