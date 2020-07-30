using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Assignments.Exceptions
{
    public class InvalidAssignmentException : Exception
    {
        public InvalidAssignmentException(string parameterName, object parameterValue)
            : base($"Invalid Classroom, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
