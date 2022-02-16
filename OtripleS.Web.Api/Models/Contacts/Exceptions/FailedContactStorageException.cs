﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Contacts.Exceptions
{
    public class FailedContactStorageException : Xeption
    {
        public FailedContactStorageException(Exception innerException)
            : base(message: "Failed contact storage error occurred, contact support.", innerException)
        { }
    }
}