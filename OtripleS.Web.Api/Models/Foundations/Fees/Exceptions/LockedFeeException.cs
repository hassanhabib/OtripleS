// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Foundations.Fees.Exceptions
{
    public class LockedFeeException : Exception
    {
        public LockedFeeException(Exception innerException)
            : base("Locked fee record exception, please try again later.", innerException) { }
    }
}
