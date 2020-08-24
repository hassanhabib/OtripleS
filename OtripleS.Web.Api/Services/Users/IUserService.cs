// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Services.Users
{
    public interface IUserService
    {
        ValueTask<User> RegisterUserAsync(User user, string password);
    }
}
