using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class AlreadyExistsCourseAttachmentException : Exception
    {
        public AlreadyExistsCourseAttachmentException(Exception innerException)
            : base("Course attachment with the same id already exists.", innerException) { }
    }
}
