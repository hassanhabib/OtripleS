using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class FailedAssignmentAttachmentStorageException : Xeption
    {
        public FailedAssignmentAttachmentStorageException(Exception innerException)
            : base(message: "Failed assignment attachment storage error occurred, contact support.", innerException)
        {}
    }
}
