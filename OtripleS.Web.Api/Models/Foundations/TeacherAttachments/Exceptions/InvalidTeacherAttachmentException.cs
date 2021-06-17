// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Foundations.TeacherAttachments.Exceptions
{
    public class InvalidTeacherAttachmentException : Exception
    {
        public InvalidTeacherAttachmentException(string parameterName, object parameterValue)
            : base($"Invalid TeacherAttachment, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
