using System;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class InvalidStudentInputException : Exception
    {
        public InvalidStudentInputException(string parameterName, object parameterValue)
            : base($"Invalid input. " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
