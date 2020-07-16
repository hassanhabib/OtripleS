using System;
namespace OtripleS.Web.Api.Models.Courses.Exceptions
{
    public class CourseDependencyException : Exception
    {
        public CourseDependencyException(Exception innerException)
            : base("Service dependency error occurred, contact support.", innerException) { }
    }
}
