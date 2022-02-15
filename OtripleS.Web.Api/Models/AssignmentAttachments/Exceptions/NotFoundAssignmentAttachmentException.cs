// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class NotFoundAssignmentAttachmentException : Exception
    {
        public NotFoundAssignmentAttachmentException(Guid assignmentId, Guid attachmentId)
            : base(message: $"Couldn't find assignment attachment with assignment id: " +
                    $"{assignmentId} " +
                    $"and attachment id: {attachmentId}.")
        { }
    }
}
