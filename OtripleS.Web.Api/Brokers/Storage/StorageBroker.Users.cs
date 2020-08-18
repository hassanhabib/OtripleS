// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public IQueryable<User> SelectAllUsers() => this.userManager.Users;

        public async System.Threading.Tasks.ValueTask<User> InsertUserAsync(User user, string password)
        {
            await this.userManager.CreateAsync(user, password);
            await this.SaveChangesAsync();

            return user;
        }
    }
}
