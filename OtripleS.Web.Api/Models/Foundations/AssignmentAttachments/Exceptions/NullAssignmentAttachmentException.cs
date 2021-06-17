// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Foundations.AssignmentAttachments.Exceptions
{
    public class NullAssignmentAttachmentException : Exception
    {
        public NullAssignmentAttachmentException() : base("The AssignmentAttachment is null.") { }
    }
}
