// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Foundations.Attachments.Exceptions
{
    public class AttachmentDependencyException : Exception
    {
        public AttachmentDependencyException(Exception innerException)
            : base("Service dependency error occurred, contact support.", innerException) { }
    }
}
