//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class AlreadyExistsCourseAttachmentException : Exception
    {
        public AlreadyExistsCourseAttachmentException(Exception innerException)
            : base(message: "Course attachment with the same id already exists.", innerException) { }
    }
}
