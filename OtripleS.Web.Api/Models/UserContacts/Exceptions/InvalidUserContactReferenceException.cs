// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.UserContacts.Exceptions
{
    public class InvalidUserContactReferenceException : Exception
    {
        public InvalidUserContactReferenceException(Exception innerException)
            : base("Invalid student contact reference error occurred.", innerException) { }
    }
}
