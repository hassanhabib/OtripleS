using System;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class NotFoundStudentSemesterCourseException : Exception
    {
        public NotFoundStudentSemesterCourseException(Guid semesterCourseId, Guid studentId)
            : base($"Couldn't find StudentSemesterCourse with semesterCourseId and studentId: {semesterCourseId} and {studentId}.") { }
    }
}