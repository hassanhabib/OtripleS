// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentAttachments.Exceptions
{
    public class AlreadyExistsStudentAttachmentException : Exception
    {
        public AlreadyExistsStudentAttachmentException(Exception innerException)
            : base(message: "Student attachment with the same id already exists.", innerException) { }
    }
}
