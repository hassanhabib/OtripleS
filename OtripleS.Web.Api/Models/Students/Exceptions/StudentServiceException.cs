using System;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class StudentServiceException : Exception
    {
        public StudentServiceException(Exception innerException)
            : base("System error occurred, contact support.", innerException)
        {

        }
    }

}