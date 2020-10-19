// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Contacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        public ValueTask<Contact> InsertContactAsync(Contact contact);
        public IQueryable<Contact> SelectAllContacts();
        public ValueTask<Contact> SelectContactByIdAsync(Guid contactId);
        public ValueTask<Contact> UpdateContactAsync(Contact contact);
        public ValueTask<Contact> DeleteContactAsync(Contact contact);
    }
}
