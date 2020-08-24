// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Brokers.UserManager
{
    public class UserManagerBroker : IUserManagerBroker
    {
        private readonly UserManager<User> userManager;

        public UserManagerBroker(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public IQueryable<User> SelectAllUsers() => this.userManager.Users;

        public async ValueTask<User> SelectUserByIdAsync(Guid userId)
        {
            return await this.userManager.FindByIdAsync(userId.ToString());
        }

        public async ValueTask<User> InsertUserAsync(User user, string password)
        {
            await this.userManager.CreateAsync(user, password);

            return user;
        }

        public async ValueTask<User> UpdateUserAsync(User user)
        {
            await this.userManager.UpdateAsync(user);

            return user;
        }

        public async ValueTask<User> DeleteUserAsync(User user)
        {
            await this.userManager.DeleteAsync(user);

            return user;
        }
    }
}
