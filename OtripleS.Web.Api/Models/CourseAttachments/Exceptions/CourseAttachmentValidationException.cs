using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class CourseAttachmentValidationException : Exception
    {
        public CourseAttachmentValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
