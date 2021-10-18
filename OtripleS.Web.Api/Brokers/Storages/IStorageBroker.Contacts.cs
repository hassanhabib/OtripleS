// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Contacts;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Contact> InsertContactAsync(Contact contact);
        IQueryable<Contact> SelectAllContacts();
        ValueTask<Contact> SelectContactByIdAsync(Guid contactId);
        ValueTask<Contact> UpdateContactAsync(Contact contact);
        ValueTask<Contact> DeleteContactAsync(Contact contact);
    }
}