using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class InvalidStudentInputException : Exception
    {
        public InvalidStudentInputException(string parameterName, object parameterValue)
            : base ($"Invalid input. " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.") { }
    }
}
