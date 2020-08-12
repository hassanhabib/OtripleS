using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class StudentSemesterCourseDependencyException: Exception
    {
        public StudentSemesterCourseDependencyException(Exception innerException) : base(
            "Service dependency error occurred, contact support.", innerException)
        {
        }
    }
}
