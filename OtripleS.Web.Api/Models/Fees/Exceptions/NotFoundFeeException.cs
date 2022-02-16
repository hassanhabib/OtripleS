// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Fees.Exceptions
{
    public class NotFoundFeeException : Exception
    {
        public NotFoundFeeException(Guid feeId)
            : base(message: $"Couldn't find fee with id: {feeId}.") { }
    }
}
