// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Foundations.AssignmentAttachments.Exceptions
{
    public class InvalidAssignmentAttachmentReferenceException : Exception
    {
        public InvalidAssignmentAttachmentReferenceException(Exception innerException)
            : base("Invalid AssignmentAttachment reference error occurred.", innerException) { }
    }
}
