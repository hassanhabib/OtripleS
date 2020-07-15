using System;
namespace OtripleS.Web.Api.Models.Courses.Exceptions
{
    public class CourseValidationException : Exception
    {
        public CourseValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
