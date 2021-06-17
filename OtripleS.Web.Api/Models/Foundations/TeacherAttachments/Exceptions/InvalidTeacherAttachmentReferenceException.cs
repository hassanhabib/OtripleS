// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Foundations.TeacherAttachments.Exceptions
{
    public class InvalidTeacherAttachmentReferenceException : Exception
    {
        public InvalidTeacherAttachmentReferenceException(Exception innerException)
            : base("Invalid teacher attachment reference error occurred.", innerException) { }
    }
}
