// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.ExamAttachments.Exceptions
{
    public class NotFoundExamAttachmentException : Exception
    {
        public NotFoundExamAttachmentException(Guid examId, Guid attachmentId)
          : base(message: $"Couldn't find exam attachment with exam id: " +
                    $"{examId} " +
                    $"and attachment id: {attachmentId}.")
        { }
    }
}
