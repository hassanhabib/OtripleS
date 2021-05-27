using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class AlreadyExistsStudentRegistrationException : Exception
    {
        public AlreadyExistsStudentRegistrationException(Exception innerException)
            : base("StudentRegistration with the same id already exists.", innerException) { }
    }
}
