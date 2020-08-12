using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class NullStudentSemesterCourseException: Exception
    {
        public NullStudentSemesterCourseException() : base("The studentsemestercourse is null.") { }
    }
}
