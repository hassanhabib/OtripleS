using System;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class TeacherAlreadyExistsException : Exception
    {
        public TeacherAlreadyExistsException(Exception innerException)
            : base("Teacher with the same id already exists.", innerException) { }
    }
}
