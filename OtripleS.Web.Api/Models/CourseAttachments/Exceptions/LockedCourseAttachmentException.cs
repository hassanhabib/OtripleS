// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class LockedCourseAttachmentException : Exception
    {
        public LockedCourseAttachmentException(Exception innerException)
            : base("Locked Course Attachment record exception, please try again later.", innerException) { }
    }
}
