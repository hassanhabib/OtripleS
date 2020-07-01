using System;

namespace SchoolEM.Models.Students.Exceptions
{
    public class NotFoundStudentException : Exception
    {
        public NotFoundStudentException(Guid studentId)
            : base($"Could not find student with ID: {studentId}") { }
    }
}
