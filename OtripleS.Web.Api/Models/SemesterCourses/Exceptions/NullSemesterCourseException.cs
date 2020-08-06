using System;
namespace OtripleS.Web.Api.Models.SemesterCourses.Exceptions
{
    public class NullSemesterCourseException : Exception
    {
        public NullSemesterCourseException() : base("The semestercourse is null.") { }
    }
}
