// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.Registrations.Exceptions
{
    public class LockedRegistrationException : Exception
    {
        public LockedRegistrationException(Exception innerException)
            : base("Locked registration record exception, please try again later.", innerException) { }
    }
}
