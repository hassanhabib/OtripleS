// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Fees.Exceptions
{
    public class InvalidFeeReferenceException : Exception
    {
        public InvalidFeeReferenceException(Exception innerException)
            : base("Invalid fee reference error occurred.", innerException) { }
    }
}
