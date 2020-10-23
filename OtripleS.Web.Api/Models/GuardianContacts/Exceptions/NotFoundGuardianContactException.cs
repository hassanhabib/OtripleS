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
           : base($"Couldn't find GuardianContact with GuardianId: {guardianId} " +
                  $"and ContactId: {contactId}.")
        { }
    }
}
