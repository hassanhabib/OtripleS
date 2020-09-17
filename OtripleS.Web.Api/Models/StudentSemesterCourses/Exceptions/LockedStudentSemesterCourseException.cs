using System;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class LockedStudentSemesterCourseException : Exception
    {
        public LockedStudentSemesterCourseException(Exception innerException)
            : base("Locked semesterCourse record exception, please try again later.", innerException) { }
    }
}
