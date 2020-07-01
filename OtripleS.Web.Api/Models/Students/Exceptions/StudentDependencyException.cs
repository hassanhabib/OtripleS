using System;

namespace SchoolEM.Models.Students.Exceptions
{
    public class StudentDependencyException : Exception
    {
        public StudentDependencyException(Exception innerException)
            : base("Service dependency error occurred, contact support", innerException)
        {

        }
    }
}
