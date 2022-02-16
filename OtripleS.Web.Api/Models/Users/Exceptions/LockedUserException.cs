﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Users.Exceptions
{
    public class LockedUserException : Exception
    {
        public LockedUserException(Exception innerException)
            : base(message: "Locked user record exception, please try again later.", innerException) { }
    }
}
