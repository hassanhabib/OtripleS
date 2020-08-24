// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.Users.Exceptions
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException(Guid userId)
            : base($"Couldn't find user with Id: {userId}.") { }
    }
}
