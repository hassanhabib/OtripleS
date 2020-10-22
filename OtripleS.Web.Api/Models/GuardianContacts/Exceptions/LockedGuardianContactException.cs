//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.GuardianContacts.Exceptions
{
    public class LockedGuardianContactException : Exception
    {
        public LockedGuardianContactException(Exception innerException)
            : base("Locked GuardianContact record exception, please try again later.", innerException) { }
    }
}