using System;
namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class StudentValidationException : Exception
    {
        public StudentValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
