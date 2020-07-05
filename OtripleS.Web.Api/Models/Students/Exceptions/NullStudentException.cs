using System;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class NullStudentException : Exception
    {
        public NullStudentException() : base($"The student is null.") { }
    }
}
