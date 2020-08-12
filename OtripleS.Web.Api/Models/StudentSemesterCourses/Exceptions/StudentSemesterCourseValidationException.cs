using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class StudentSemesterCourseValidationException: Exception
    {
        public StudentSemesterCourseValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException)
        {
        }
    }
}
