// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.UserContacts.Exceptions
{
    public class NotFoundUserContactException : Exception
    {
        public NotFoundUserContactException(Guid userId, Guid contactId)
           : base(message: $"Couldn't find user contact with user id: {userId} " +
                  $"and contact id: {contactId}.")
        { }
    }
}