// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.GuardianAttachments.Exceptions
{
    public class GuardianAttachmentValidationException : Exception
    {
        public GuardianAttachmentValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
    }
}
