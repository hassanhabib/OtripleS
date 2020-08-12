using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class InvalidStudentSemesterCourseException: Exception
    {
        public InvalidStudentSemesterCourseException(string parameterName, object parameterValue)
            : base($"Invalid SemesterCourse, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
