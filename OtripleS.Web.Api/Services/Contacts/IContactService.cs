// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Contacts;

namespace OtripleS.Web.Api.Services.Contacts
{
    public interface IContactService
    {
        ValueTask<Contact> AddContactAsync(Contact contact);
        IQueryable<Contact> RetrieveAllContacts();
        ValueTask<Contact> RetrieveContactByIdAsync(Guid contactId);
        ValueTask<Contact> ModifyContactAsync(Contact contact);
        ValueTask<Contact> RemoveContactByIdAsync(Guid contactId);
    }
}
