//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.ExamAttachments.Exceptions
{
    public class NotFoundExamAttachmentException : Exception
    {
        public NotFoundExamAttachmentException(Guid examId, Guid attachmentId)
          : base($"Couldn't find Exam Attachment with examId: " +
                    $"{examId} " +
                    $"and attachmentId: {attachmentId}.")
        { }
    }
}
