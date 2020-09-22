// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Contacts.Exceptions
{
    public class AlreadyExistsContactException : Exception
    {
        public AlreadyExistsContactException(Exception innerException)
            : base("Contact with the same id already exists.", innerException) { }
    }
}
