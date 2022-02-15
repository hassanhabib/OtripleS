// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.UserContacts.Exceptions
{
    public class LockedUserContactException : Exception
    {
        public LockedUserContactException(Exception innerException)
            : base(message: "Locked user contact record exception, please try again later.", innerException) { }
    }
}