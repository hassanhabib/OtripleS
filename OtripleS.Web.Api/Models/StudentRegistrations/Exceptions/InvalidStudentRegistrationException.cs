using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class InvalidStudentRegistrationException : Exception
    {
        public InvalidStudentRegistrationException(string parameterName, object parameterValue)
            : base($"Invalid StudentRegistration, " +
                  $"ParameterNmae: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}