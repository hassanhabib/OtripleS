using System;

namespace OtripleS.Web.Api.Models.CourseAttachments.Exceptions
{
    public class InvalidCourseAttachmentReferenceException : Exception
    {
        public InvalidCourseAttachmentReferenceException(Exception innerException)
            : base("Invalid course attachment reference error occurred.", innerException) { }
    }
}
