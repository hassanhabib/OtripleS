using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Courses.Exceptions
{
    public class FailedCourseServiceException : Xeption
    {
        public FailedCourseServiceException(Exception innerException)
            : base(message: " Failed course service error occured, contact support.",
                  innerException)
        { }
    }
}
