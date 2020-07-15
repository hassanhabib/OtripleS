using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Courses.Exceptions
{
    public class NullCourseException : Exception
    {
        public NullCourseException() :base("The course is null.")
        {
        }
    }
}
