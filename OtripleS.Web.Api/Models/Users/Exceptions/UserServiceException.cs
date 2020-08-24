// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.Users.Exceptions
{
    public class UserServiceException : Exception
    {
        public UserServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
