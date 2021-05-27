using System;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class StudentRegistrationServiceException : Exception
    {
        public StudentRegistrationServiceException(Exception innerException)
            : base("Service error occured, contact support.", innerException)
        { }
    }
}
