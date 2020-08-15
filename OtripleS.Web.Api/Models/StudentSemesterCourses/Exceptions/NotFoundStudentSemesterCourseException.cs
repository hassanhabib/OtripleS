using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class NotFoundStudentSemesterCourseException:Exception
    {
        public NotFoundStudentSemesterCourseException(Guid studentId, Guid semesterCourseId)
           : base($"Couldn't find StudentSemesterCourse with StudentId: {studentId} " +
                  $"and SemesterCourseId: {semesterCourseId}.") { }
    }
}
