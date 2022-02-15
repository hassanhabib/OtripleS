// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class FailedAssignmentAttachmentStorageException : Xeption
    {
        public FailedAssignmentAttachmentStorageException(Exception innerException)
            : base(message: "Failed assignment attachment storage error occurred, contact support.", innerException)
        { }
    }
}
