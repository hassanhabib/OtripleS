// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.ExamFees.Exceptions
{
    public class LockedExamFeeException : Exception
    {
        public LockedExamFeeException(Exception innerException)
            : base(message: "Locked exam fee record exception, please try again later.", innerException) { }
    }
}
