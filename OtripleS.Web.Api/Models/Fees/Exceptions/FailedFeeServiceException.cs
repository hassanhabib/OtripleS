// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------
using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Fees.Exceptions
{
    public class FailedFeeServiceException : Xeption
    {
        public FailedFeeServiceException(Exception innerException)
            : base(message: " Failed fee service error occurred, contact support",
                  innerException)
        { }
    }
}
