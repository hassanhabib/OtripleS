// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Users.Exceptions
{
    public class FailedUserStorageException : Xeption
    {
        public FailedUserStorageException(Exception innerException)
            : base(message: "Failed user storage error occurred, contact suppport.",
                  innerException)
        { }
    }
}
