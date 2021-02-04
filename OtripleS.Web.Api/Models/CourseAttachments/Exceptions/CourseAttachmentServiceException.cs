using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class CourseAttachmentServiceException : Exception
    {
        public CourseAttachmentServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
