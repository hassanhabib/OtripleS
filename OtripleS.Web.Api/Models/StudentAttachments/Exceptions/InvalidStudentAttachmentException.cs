//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentAttachments.Exceptions
{
    public class InvalidStudentAttachmentException : Exception
    {
        public InvalidStudentAttachmentException(string parameterName, object parameterValue)
            : base(message: $"Invalid student attachment, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }
    }
}
