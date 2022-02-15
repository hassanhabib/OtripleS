// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Registrations.Exceptions
{
    public class NullRegistrationException : Exception
    {
        public NullRegistrationException() : base(message: "The registration is null.") { }
    }
}
