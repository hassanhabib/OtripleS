using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class NullCourseAttachmentException : Exception
    {
        public NullCourseAttachmentException() : base("The course attachment is null.") { }
    }
}
