// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.ExamFees.Exceptions
{
    public class InvalidExamFeeException : Xeption
    {
        public InvalidExamFeeException(string parameterName, object parameterValue)
           : base($"Invalid ExamFee, " +
                 $"ParameterName: {parameterName}, " +
                 $"ParameterValue: {parameterValue}.")
        { }

        public InvalidExamFeeException()
            : base("Invalid exam fee. Please fix the errors and try again.") { }
    }
}
