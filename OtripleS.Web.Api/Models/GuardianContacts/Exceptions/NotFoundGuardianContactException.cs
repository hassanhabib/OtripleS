// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.GuardianContacts.Exceptions
{
    public class NotFoundGuardianContactException : Exception
    {
        public NotFoundGuardianContactException(Guid guardianId, Guid contactId)
            : base(message: $"Couldn't find guardian contact with guardian id: {guardianId} " +
                  $"and contact id: {contactId}.")
        { }
    }
}
