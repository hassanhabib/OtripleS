// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentAttachments.Exceptions
{
    public class NullStudentAttachmentException : Exception
    {
        public NullStudentAttachmentException() : base("The StudentAttachment is null.") { }
    }
}
