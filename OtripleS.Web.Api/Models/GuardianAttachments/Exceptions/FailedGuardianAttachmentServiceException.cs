// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.GuardianAttachments.Exceptions
{
    public class FailedGuardianAttachmentServiceException : Xeption
    {
        public FailedGuardianAttachmentServiceException(Exception innerException)
            : base(message: "Failed guardian attachment service error occurred, contact support",
                 innerException)
        { }
    }
}
