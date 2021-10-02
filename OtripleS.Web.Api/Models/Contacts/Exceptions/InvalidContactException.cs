// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Contacts.Exceptions
{
    public class InvalidContactException : Xeption
    {
        public InvalidContactException(string parameterName, object parameterValue)
            : base($"Invalid Contact, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }

        public InvalidContactException()
            : base("Invalid contact. Please fix the errors and try again.") { }
    }
}
