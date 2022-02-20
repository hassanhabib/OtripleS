// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.ExamFees.Exceptions
{
    public class FailedExamFeeServiceException : Xeption
    {
        public FailedExamFeeServiceException(Exception innerException)
            : base(message: " Failed exam fee service error occured, contact support",
                  innerException)
        { }
    }
}
