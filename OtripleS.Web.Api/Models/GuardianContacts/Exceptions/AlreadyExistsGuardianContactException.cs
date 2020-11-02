// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.GuardianContacts.Exceptions
{
    public class AlreadyExistsGuardianContactException : Exception
    {
        public AlreadyExistsGuardianContactException(Exception innerException)
            : base("Guardian Contact with the same id already exists.", innerException) { }
    }
}
