// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.UserContacts.Exceptions
{
    public class InvalidUserContactException : Xeption
    {
        public InvalidUserContactException()
            : base(message: $"Invalid user contact. Please fix the errors and try again.") { }
    }
}
