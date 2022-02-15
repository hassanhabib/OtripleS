// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Users;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string UsersRelativeUrl = "api/users";

        public async ValueTask<User> PostUserAsync(User user) =>
            await this.apiFactoryClient.PostContentAsync(UsersRelativeUrl, user);

        public async ValueTask<User> GetUserByIdAsync(Guid userId) =>
            await this.apiFactoryClient.GetContentAsync<User>($"{UsersRelativeUrl}/{userId}");

        public async ValueTask<User> DeleteUserByIdAsync(Guid userId) =>
            await this.apiFactoryClient.DeleteContentAsync<User>($"{UsersRelativeUrl}/{userId}");

        public async ValueTask<User> PutUserAsync(User user) =>
            await this.apiFactoryClient.PutContentAsync(UsersRelativeUrl, user);

        public async ValueTask<List<User>> GetAllUsersAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<User>>($"{UsersRelativeUrl}/");
    }
}
