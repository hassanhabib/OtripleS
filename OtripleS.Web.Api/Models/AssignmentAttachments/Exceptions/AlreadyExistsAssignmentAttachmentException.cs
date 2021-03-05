// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class AlreadyExistsAssignmentAttachmentException : Exception
    {
        public AlreadyExistsAssignmentAttachmentException(Exception innerException)
            : base("AssignmentAttachment with the same id already exists.", innerException) { }
    }
}
