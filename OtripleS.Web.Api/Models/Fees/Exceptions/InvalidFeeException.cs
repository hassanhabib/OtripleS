// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Fees.Exceptions
{
    public class InvalidFeeException : Xeption
    {
        public InvalidFeeException()
            : base("Fee is invalid. Please fix the errors and try again.")
        { }
    }
}
