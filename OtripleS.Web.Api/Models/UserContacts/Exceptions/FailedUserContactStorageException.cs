// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.UserContacts.Exceptions
{
    public class FailedUserContactStorageException : Xeption
    {
        public FailedUserContactStorageException(Exception innerException)
            : base(message: "Failed user contact storage error occurred, contact suppport.", innerException)
        {}
    }
}
