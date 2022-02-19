using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.ExamFees.Exceptions
{
    public class FailedExamFeeServiceException : Xeption
    {
        public FailedExamFeeServiceException(Exception innerException)
            : base(message: " Failed exam fee service error occured, contact support",
                  innerException)
        { }
    }
}
