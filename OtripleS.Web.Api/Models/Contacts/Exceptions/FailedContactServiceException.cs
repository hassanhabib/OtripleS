// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Contacts.Exceptions
{
    public class FailedContactServiceException : Xeption
    {
        public FailedContactServiceException(Exception innerException)
            : base(message: "Failed contact service error occured, contact support",
                 innerException)
        { }
    }
}
