using System;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class InvalidStudentException : Exception
    {
        public InvalidStudentException(string parameterName, object parameterValue)
            : base($"Invalid student error occurred. parameter name: {parameterName} " +
                   $"parameter value: {parameterValue}")
        { }
    }

}