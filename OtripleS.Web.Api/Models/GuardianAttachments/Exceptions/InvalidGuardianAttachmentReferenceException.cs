//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.GuardianAttachments.Exceptions
{
    public class InvalidGuardianAttachmentReferenceException : Exception
    {
        public InvalidGuardianAttachmentReferenceException(Exception innerException)
            : base("Invalid Guardian Attachment reference error occurred.", innerException) { }
    }
}
