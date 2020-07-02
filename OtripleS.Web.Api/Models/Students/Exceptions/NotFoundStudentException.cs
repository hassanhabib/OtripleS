using System;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class NotFoundStudentException : Exception
    {
        public NotFoundStudentException(Guid studentId)
            : base($"Could not find student with ID: {studentId}") { }
    }
}