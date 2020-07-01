using System;

namespace SchoolEM.Models.Students.Exceptions
{
    public class StudentValidationException : Exception
    {
        public StudentValidationException(Exception innerException)
            : base("Student validation error occurred, correct your request and try again", innerException)
        { }

    }
}
