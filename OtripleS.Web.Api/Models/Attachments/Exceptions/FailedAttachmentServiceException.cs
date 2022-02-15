using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Attachments.Exceptions
{
    public class FailedAttachmentServiceException : Xeption
    {
        public FailedAttachmentServiceException(Exception innerException)
            : base(message: "Failed Attachment Service Exception occured,contact support", innerException)
        { }
    }
}
