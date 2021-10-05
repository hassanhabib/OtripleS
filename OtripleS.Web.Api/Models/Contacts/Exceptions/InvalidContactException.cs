// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Contacts.Exceptions
{
    public class InvalidContactException : Xeption
    {
        public InvalidContactException(string parameterName, object parameterValue)
            : base(message: $"Invalid Contact, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }

        public InvalidContactException()
            : base(message: "Invalid contact. Please fix the errors and try again.") { }
    }
}
