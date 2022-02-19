using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.ExamAttachments.Exceptions
{
    public class FailedExamAttachmentServiceException: Xeption
    {
        public FailedExamAttachmentServiceException(Exception innerException)
            : base(message: "Failed exam attachment service error occured, contact support",
                  innerException)
        { }
    }
}
