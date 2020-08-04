using System;

namespace OtripleS.Web.Api.Models.SemesterCourses.Exceptions
{
    public class NotFoundSemesterCourseException : Exception
    {
        public NotFoundSemesterCourseException(Guid semesterCourseId)
            : base($"Couldn't find SemesterCourse with Id: {semesterCourseId}.") { }
    }
}