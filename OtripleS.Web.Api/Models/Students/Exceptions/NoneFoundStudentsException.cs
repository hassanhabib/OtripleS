using System;

namespace SchoolEM.Models.Students.Exceptions
{
    public class NoneFoundStudentsException : Exception
    {
        public NoneFoundStudentsException()
            : base($"Could not find any students") { }
    }
}
