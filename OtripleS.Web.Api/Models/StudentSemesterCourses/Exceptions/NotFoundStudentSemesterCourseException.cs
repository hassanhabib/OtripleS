using System;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class NotFoundStudentSemesterCourseException : Exception
    {
        public NotFoundStudentSemesterCourseException(Guid studentId, Guid semesterCourseId)
           : base($"Couldn't find StudentSemesterCourse with StudentId: {studentId} " +
                  $"and SemesterCourseId: {semesterCourseId}.")
        { }
    }
}
