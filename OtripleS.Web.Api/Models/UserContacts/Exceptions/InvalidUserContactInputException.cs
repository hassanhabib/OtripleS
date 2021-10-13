//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.UserContacts.Exceptions
{
    public class InvalidUserContactInputException : Xeption
    {
        public InvalidUserContactInputException(string parameterName, object parameterValue)
            : base(message: $"Invalid user contact, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }

        public InvalidUserContactInputException()
            : base(message: $"Invalid user contact. Please fix the errors and try again.") { }
    }
}
