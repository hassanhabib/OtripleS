using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.ExamAttachments.Exceptions
{
    public class ExamAttachmentServiceException : Exception
    {
        public ExamAttachmentServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
