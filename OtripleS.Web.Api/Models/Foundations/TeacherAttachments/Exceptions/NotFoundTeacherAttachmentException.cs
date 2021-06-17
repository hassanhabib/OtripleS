// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Foundations.TeacherAttachments.Exceptions
{
    public class NotFoundTeacherAttachmentException : Exception
    {
        public NotFoundTeacherAttachmentException(Guid teacherId, Guid attachmentId)
           : base($"Couldn't find teacherAttachment with teacherId: {teacherId} " +
                  $"and attachmentId: {attachmentId}.")
        { }
    }
}
