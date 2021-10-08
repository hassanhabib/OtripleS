// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class InvalidStudentExamFeeException : Exception
    {
        public InvalidStudentExamFeeException(string parameterName, object parameterValue)
            : base(message: $"Invalid student exam fee, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }
    }
}
