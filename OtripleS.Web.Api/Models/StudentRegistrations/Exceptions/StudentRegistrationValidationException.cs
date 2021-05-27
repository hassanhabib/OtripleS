using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class StudentRegistrationValidationException : Exception
    {
        public StudentRegistrationValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
