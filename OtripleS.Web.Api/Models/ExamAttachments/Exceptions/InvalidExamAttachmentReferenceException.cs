// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.ExamAttachments.Exceptions
{
    public class InvalidExamAttachmentReferenceException : Exception
    {
        public InvalidExamAttachmentReferenceException(Exception innerException)
            : base(message: "Invalid exam attachment reference error occurred.", innerException) { }
    }
}
