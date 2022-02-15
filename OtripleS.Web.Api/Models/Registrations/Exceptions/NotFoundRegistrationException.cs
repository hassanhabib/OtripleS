// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Registrations.Exceptions
{
    public class NotFoundRegistrationException : Exception
    {
        public NotFoundRegistrationException(Guid registrationId)
            : base(message: $"Couldn't find fee with id: {registrationId}.") { }
    }
}
