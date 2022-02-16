﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Contacts.Exceptions
{
    public class NotFoundContactException : Exception
    {
        public NotFoundContactException(Guid contactId)
            : base(message: $"Couldn't find contact with id: {contactId}.") { }
    }
}
