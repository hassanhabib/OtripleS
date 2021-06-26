// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Contacts.Exceptions
{
    public class ContactValidationException : Exception
    {
        public ContactValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
