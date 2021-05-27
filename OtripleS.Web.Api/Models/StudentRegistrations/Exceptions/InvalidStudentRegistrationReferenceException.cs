using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class InvalidStudentRegistrationReferenceException : Exception
    {
        public InvalidStudentRegistrationReferenceException(Exception innerException)
            : base("Invalid student registration reference error occurred.", innerException) { }
    }
}
