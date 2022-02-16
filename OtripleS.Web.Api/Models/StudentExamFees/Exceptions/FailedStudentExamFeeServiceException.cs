// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class FailedStudentExamFeeServiceException : Xeption
    {
        public FailedStudentExamFeeServiceException(Exception innerException)
            : base(message: "Failed student exam fee service error occured." ,innerException)
        {
        }
    }
}
