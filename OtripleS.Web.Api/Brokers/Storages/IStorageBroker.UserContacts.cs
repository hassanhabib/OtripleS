// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.UserContacts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<UserContact> InsertUserContactAsync(
           UserContact userContact);

        IQueryable<UserContact> SelectAllUserContacts();

        ValueTask<UserContact> SelectUserContactByIdAsync(
           Guid userId,
           Guid contactId);

        ValueTask<UserContact> UpdateUserContactAsync(
           UserContact userContact);

        ValueTask<UserContact> DeleteUserContactAsync(
           UserContact userContact);
    }
}