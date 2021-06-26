//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.GuardianAttachments.Exceptions
{
    public class NotFoundGuardianAttachmentException : Exception
    {
        public NotFoundGuardianAttachmentException(Guid guardianId, Guid attachmentId)
           : base($"Couldn't find guardianAttachment with studentId: {guardianId} " +
                  $"and attachmentId: {attachmentId}.")
        { }
    }
}
