using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class NullStudentRegistrationException : Exception
    {
        public NullStudentRegistrationException() : base("The StudentRegistration is null.") { }
    }
}
