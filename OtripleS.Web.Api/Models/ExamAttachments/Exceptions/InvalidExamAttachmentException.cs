// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.ExamAttachments.Exceptions
{
    public class InvalidExamAttachmentException : Xeption
    {
        public InvalidExamAttachmentException(string parameterName, object parameterValue)
           : base(message: $"Invalid exam attachment, " +
                 $"parameter name: {parameterName}, " +
                 $"parameter value: {parameterValue}.")
        { }

        public InvalidExamAttachmentException()
            : base(message: "Invalid exam attachment. Please fix the errors and try again.") { }
    }
}
