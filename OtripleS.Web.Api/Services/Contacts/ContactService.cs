// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Contacts;

namespace OtripleS.Web.Api.Services.Contacts
{
    public partial class ContactService : IContactService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ContactService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Contact> AddContactAsync(Contact contact) =>
        TryCatch(async () =>
        {
            ValidateContactOnCreate(contact);

            return await this.storageBroker.InsertContactAsync(contact);
        });

        public IQueryable<Contact> RetrieveAllContacts() =>
        TryCatch(() =>
        {
            IQueryable<Contact> storageContacts = this.storageBroker.SelectAllContacts();
            ValidateStorageContacts(storageContacts);

            return storageContacts;
        });

        public ValueTask<Contact> RetrieveContactByIdAsync(Guid contactId) =>
        TryCatch(async () =>
        {
            ValidateContactId(contactId);
            Contact contact = await this.storageBroker.SelectContactByIdAsync(contactId);
            ValidateStorageContact(contact, contactId);
            return contact;
        });

        public ValueTask<Contact> ModifyContactAsync(Contact contact) =>
        TryCatch(async () =>
        {
            ValidateContactOnModify(contact);
            Contact maybeContact = await storageBroker.SelectContactByIdAsync(contact.Id);
            ValidateStorageContact(maybeContact, contact.Id);
            ValidateAgainstStorageContactOnModify(inputContact: contact, storageContact: maybeContact);

            return await storageBroker.UpdateContactAsync(contact);
        });

        public ValueTask<Contact> RemoveContactByIdAsync(Guid contactId) =>
        TryCatch(async () =>
        {
            ValidateContactId(contactId);
            Contact maybeContact = await this.storageBroker.SelectContactByIdAsync(contactId);
            ValidateStorageContact(maybeContact, contactId);

            return await this.storageBroker.DeleteContactAsync(maybeContact);
        });
    }
}
