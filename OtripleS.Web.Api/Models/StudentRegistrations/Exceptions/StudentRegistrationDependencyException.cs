using System;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class StudentRegistrationDependencyException : Exception
    {
        public StudentRegistrationDependencyException(Exception innerException)
            : base("Service dependency error occured, contact support.", innerException)
        { }
    }
}
