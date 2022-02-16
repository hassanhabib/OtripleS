﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.UserContacts.Exceptions
{
    public class AlreadyExistsUserContactException : Exception
    {
        public AlreadyExistsUserContactException(Exception innerException)
            : base(message: "User contact with the same id already exists.", innerException) { }
    }
}
