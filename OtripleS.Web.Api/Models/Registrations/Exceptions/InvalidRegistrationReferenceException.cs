// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Registrations.Exceptions
{
    public class InvalidRegistrationReferenceException : Exception
    {
        public InvalidRegistrationReferenceException(Exception innerException)
            : base(message: "Invalid registration reference error occurred.", innerException) { }
    }
}
