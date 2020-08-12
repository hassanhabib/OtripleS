using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class StudentSemesterCourseServiceException: Exception
    {
        public StudentSemesterCourseServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException)
        {
        }
    }
}
