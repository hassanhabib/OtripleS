// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Brokers.UserManagement
{
    public class UserManagementBroker : IUserManagementBroker
    {
        private readonly UserManager<User> userManagement;

        public UserManagementBroker(UserManager<User> userManager)
        {
            this.userManagement = userManager;
        }
        public IQueryable<User> SelectAllUsers() => this.userManagement.Users;

        public async ValueTask<User> SelectUserByIdAsync(Guid userId)
        {
            return await this.userManagement.FindByIdAsync(userId.ToString());
        }

        public async ValueTask<User> InsertUserAsync(User user, string password)
        {
            await this.userManagement.CreateAsync(user, password);

            return user;
        }

        public async ValueTask<User> UpdateUserAsync(User user)
        {
            await this.userManagement.UpdateAsync(user);

            return user;
        }

        public async ValueTask<User> DeleteUserAsync(User user)
        {
            await this.userManagement.DeleteAsync(user);

            return user;
        }
    }
}
