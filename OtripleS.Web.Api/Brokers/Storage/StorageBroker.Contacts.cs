// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Contacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Contact> Contacts { get; set; }

        public async ValueTask<Contact> InsertContactAsync(Contact contact)
        {
            EntityEntry<Contact> contactEntityEntry = await this.Contacts.AddAsync(contact);
            await this.SaveChangesAsync();

            return contactEntityEntry.Entity;
        }

        public IQueryable<Contact> SelectAllContacts() => this.Contacts.AsQueryable();

        public async ValueTask<Contact> SelectContactByIdAsync(Guid contactId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Contacts.FindAsync(contactId);
        }

        public async ValueTask<Contact> UpdateContactAsync(Contact contact)
        {
            EntityEntry<Contact> contactEntityEntry = this.Contacts.Update(contact);
            await this.SaveChangesAsync();

            return contactEntityEntry.Entity;
        }

        public async ValueTask<Contact> DeleteContactAsync(Contact contact)
        {
            EntityEntry<Contact> contactEntityEntry = this.Contacts.Remove(contact);
            await this.SaveChangesAsync();

            return contactEntityEntry.Entity;
        }
    }
}
