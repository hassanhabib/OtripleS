using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Guardian.Exceptions
{
    public class InvalidGuardianInputException:Exception
    {
        public InvalidGuardianInputException(string parameterName ,object parameterValue)
        : base($"Invalid Guardian, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
