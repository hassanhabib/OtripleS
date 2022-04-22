// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.UserManagement;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Services.Foundations.Users
{
    public partial class UserService : IUserService
    {
        private readonly IUserManagementBroker userManagementBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserService(
            IUserManagementBroker userManagementBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.userManagementBroker = userManagementBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<User> RegisterUserAsync(User user, string password) =>
        TryCatch(async () =>
        {
            ValidateUserOnCreate(user, password);

            return await this.userManagementBroker.InsertUserAsync(user, password);
        });

        public IQueryable<User> RetrieveAllUsers() =>
        TryCatch(() => this.userManagementBroker.SelectAllUsers());

        public ValueTask<User> RetrieveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserIdIsNull(userId);
            User storageUser = await this.userManagementBroker.SelectUserByIdAsync(userId);
            ValidateStorageUser(storageUser, userId);

            return storageUser;
        });

        public ValueTask<User> ModifyUserAsync(User user) =>
        TryCatch(async () =>
        {
            ValidateUserOnModify(user);
            User maybeUser = await this.userManagementBroker.SelectUserByIdAsync(user.Id);
            ValidateStorageUser(maybeUser, user.Id);
            ValidateAgainstStorageUserOnModify(inputUser: user, storageUser: maybeUser);

            return await this.userManagementBroker.UpdateUserAsync(user);
        });

        public ValueTask<User> RemoveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserIdIsNull(userId);
            User maybeUser = await this.userManagementBroker.SelectUserByIdAsync(userId);
            ValidateStorageUser(maybeUser, userId);

            return await this.userManagementBroker.DeleteUserAsync(maybeUser);
        });
    }
}
