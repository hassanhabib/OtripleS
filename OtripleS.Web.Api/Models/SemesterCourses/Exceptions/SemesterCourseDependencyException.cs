using System;

namespace OtripleS.Web.Api.Models.SemesterCourses.Exceptions
{
    public class SemesterCourseDependencyException : Exception
    {
        public SemesterCourseDependencyException(Exception innerException) : base(
            "Service dependency error occurred, contact support.", innerException)
        {
        }
    }
}