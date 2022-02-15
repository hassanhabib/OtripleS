// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.TeacherAttachments.Exceptions
{
    public class NullTeacherAttachmentException : Exception
    {
        public NullTeacherAttachmentException() : base(message: "The teacher attachment is null.") { }
    }
}
