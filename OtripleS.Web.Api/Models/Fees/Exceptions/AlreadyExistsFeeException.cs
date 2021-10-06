// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Fees.Exceptions
{
    public class AlreadyExistsFeeException : Exception
    {
        public AlreadyExistsFeeException(Exception innerException)
            : base(message: "Fee with the same id already exists.", innerException) { }
    }
}
