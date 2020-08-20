// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Models.Users.Exceptions;

namespace OtripleS.Web.Api.Services.Users
{
    public partial class UserService
    {
        private void ValidateUserOnCreate(User user, string password)
        {
            ValidateUserIsNull(user);
        }

        private void ValidateUserIsNull(User user)
        {
            if (user is null)
            {
                throw new NullUserException();
            }
        }
    }
}
