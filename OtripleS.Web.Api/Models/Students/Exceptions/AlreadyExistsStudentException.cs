using System;

namespace SchoolEM.Models.Students.Exceptions
{
    public class AlreadyExistsStudentException : Exception
    {
        public AlreadyExistsStudentException(Exception innerException)
            : base("This student already exists", innerException)
        { }
    }
}
