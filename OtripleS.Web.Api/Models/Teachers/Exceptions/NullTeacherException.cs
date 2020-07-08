using System;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class NullTeacherException : Exception
    {
        public NullTeacherException() : base($"The teacher is null.") { }
    }
}
