using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class AlreadyExistsStudentSemesterCourseException: Exception
    {
        public AlreadyExistsStudentSemesterCourseException(Exception innerException)
            : base("StudentSemesterCourse with the same id already exists.", innerException) { }
    }
}
