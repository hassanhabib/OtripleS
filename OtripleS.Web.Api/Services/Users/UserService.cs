// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.UserManagement;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Services.Users
{
    public partial class UserService : IUserService
    {
        private readonly IUserManagementBroker userManagementBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public UserService(IUserManagementBroker userManagementBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.userManagementBroker = userManagementBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<User> DeleteUserAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserIdIsNull(userId);

            User maybeUser =
               await this.userManagementBroker.SelectUserByIdAsync(userId);

            ValidateStorageUser(maybeUser, userId);

            return await this.userManagementBroker.DeleteUserAsync(maybeUser);
        });

        public ValueTask<User> ModifyUserAsync(User user) =>
        TryCatch(async () =>
        {
            ValidateUserOnModify(user);
            User maybeUser = await this.userManagementBroker.SelectUserByIdAsync(user.Id);

            return await this.userManagementBroker.UpdateUserAsync(user);
        });

        public ValueTask<User> RegisterUserAsync(User user, string password) =>
        TryCatch(async () =>
        {
            ValidateUserOnCreate(user, password);

            return await this.userManagementBroker.InsertUserAsync(user, password);
        });

        public IQueryable<User> RetrieveAllUsers() =>
        TryCatch(() =>
        {
            IQueryable<User> storageUsers = this.userManagementBroker.SelectAllUsers();
            ValidateStorageUsers(storageUsers);

            return storageUsers;
        });

        public ValueTask<User> RetrieveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserIdIsNull(userId);
            User storageUser = await this.userManagementBroker.SelectUserByIdAsync(userId);
            ValidateStorageUser(storageUser, userId);

            return storageUser;
        });
    }
}
