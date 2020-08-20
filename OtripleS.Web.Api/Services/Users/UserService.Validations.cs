// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Models.Users.Exceptions;

namespace OtripleS.Web.Api.Services.Users
{
    public partial class UserService
    {
        private void ValidateUserOnCreate(User user, string password)
        {
            ValidateUserIsNull(user);
            ValidateUserIdIsNull(user.Id);
            ValidateUserFields(user);
        }

        private void ValidateUserFields(User user)
        {
            if (IsInvalid(user.UserName))
            {
                throw new InvalidUserException(
                    parameterName: nameof(User.UserName),
                    parameterValue: user.UserName);
            }
        }

        private void ValidateUserIdIsNull(Guid userId)
        {
            if (userId == default)
            {
                throw new InvalidUserException(
                    parameterName: nameof(User.Id),
                    parameterValue: userId);
            }
        }

        private void ValidateUserIsNull(User user)
        {
            if (user is null)
            {
                throw new NullUserException();
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
    }
}
