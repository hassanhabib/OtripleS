﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Users.Exceptions
{
    public class AlreadyExistsUserException : Exception
    {
        public AlreadyExistsUserException(Exception innerException)
            : base(message: "User with the same id already exists.", innerException) { }
    }
}
