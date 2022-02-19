using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class FailedExamServiceException : Xeption
    {
        public FailedExamServiceException(Exception innerException)
            : base(message:"Failed exam service error occured, contact support",
                  innerException)
        { }
    }
}
