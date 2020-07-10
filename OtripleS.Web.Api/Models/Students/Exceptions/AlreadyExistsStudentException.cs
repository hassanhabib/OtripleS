using System;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class AlreadyExistsStudentException : Exception
    {
        public AlreadyExistsStudentException(Exception innerException)
            : base("Student with the same id already exists.", innerException) { }
    }
}
