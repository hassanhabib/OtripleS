// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.UserContacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        public ValueTask<UserContact> InsertUserContactAsync(
            UserContact userContact);

        public IQueryable<UserContact> SelectAllUserContacts();

        public ValueTask<UserContact> SelectUserContactByIdAsync(
            Guid userId,
            Guid contactId);

        public ValueTask<UserContact> UpdateUserContactAsync(
            UserContact userContact);

        public ValueTask<UserContact> DeleteUserContactAsync(
            UserContact userContact);
    }
}
