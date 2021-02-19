// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.ExamAttachments.Exceptions
{
    public class NotFoundExamAttachmentException : Exception
    {
        public NotFoundExamAttachmentException(Guid guardianId, Guid attachmentId)
           : base($"Couldn't find ExamAttachment with examId: {guardianId} " +
                  $"and attachmentId: {attachmentId}.")
        { }
    }
}
