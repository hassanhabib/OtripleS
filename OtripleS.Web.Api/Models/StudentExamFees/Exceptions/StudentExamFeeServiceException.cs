using System;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class StudentExamFeeServiceException : Exception
    {
        public StudentExamFeeServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
