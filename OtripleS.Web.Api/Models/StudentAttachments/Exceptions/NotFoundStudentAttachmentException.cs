// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentAttachments.Exceptions
{
    public class NotFoundStudentAttachmentException : Exception
    {
        public NotFoundStudentAttachmentException(Guid studentId, Guid attachmentId)
           : base(message: $"Couldn't find student attachment with student id: {studentId} " +
                  $"and attachment id: {attachmentId}.")
        { }
    }
}
