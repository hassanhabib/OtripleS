// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.Users.Exceptions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException(string parameterName, object parameterValue)
            : base($"Invalid User, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
