// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.GuardianAttachments.Exceptions
{
    public class LockedGuardianAttachmentException : Exception
    {
        public LockedGuardianAttachmentException(Exception innerException)
            : base(message: "Locked guardian attachment record exception, please try again later.", innerException) { }
    }
}
