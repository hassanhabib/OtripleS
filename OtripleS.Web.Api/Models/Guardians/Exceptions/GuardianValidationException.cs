// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Guardians.Exceptions
{
    public class GuardianValidationException : Exception
    {
        public GuardianValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
