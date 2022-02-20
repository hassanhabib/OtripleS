// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.CoursesAttachments.Exceptions
{
    public class FailedCourseAttachmentServiceException : Xeption
    {
        public FailedCourseAttachmentServiceException(Exception innerException)
             : base(message: "Failed course attachment service error occured, contact support.",
                  innerException)
        { }
    }
}
