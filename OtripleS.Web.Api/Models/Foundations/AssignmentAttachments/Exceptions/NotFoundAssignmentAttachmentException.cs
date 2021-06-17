//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Foundations.AssignmentAttachments.Exceptions
{
    public class NotFoundAssignmentAttachmentException : Exception
    {
        public NotFoundAssignmentAttachmentException(Guid assignmentId, Guid attachmentId)
            : base($"Couldn't find Assignment Attachment with assignmentId: " +
                    $"{assignmentId} " +
                    $"and attachmentId: {attachmentId}.")
        { }
    }
}
