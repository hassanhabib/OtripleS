using System;

namespace SchoolEM.Models.Students.Exceptions
{
    public class NullStudentException : Exception
    {
        public NullStudentException() : base("Student is Null") { }
    }
}
