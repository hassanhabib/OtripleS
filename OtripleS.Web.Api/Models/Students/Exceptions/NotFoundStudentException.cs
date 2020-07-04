using System;
namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class NotFoundStudentException : Exception
    {
        public NotFoundStudentException(Guid studentId)
            : base($"Couldn't find student with Id: {studentId}.") { }
    }
}
