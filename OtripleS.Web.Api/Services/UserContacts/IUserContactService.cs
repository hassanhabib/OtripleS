// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.UserContacts;

namespace OtripleS.Web.Api.Services.UserContacts
{
    public interface IUserContactService
    {
        ValueTask<UserContact> AddUserContactAsync(UserContact userContact);
        IQueryable<UserContact> RetrieveAllUserContacts();
        ValueTask<UserContact> RetrieveUserContactByIdAsync(Guid userId, Guid contactId);
        ValueTask<UserContact> RemoveUserContactByIdAsync(Guid userId, Guid contactId);
    }
}
