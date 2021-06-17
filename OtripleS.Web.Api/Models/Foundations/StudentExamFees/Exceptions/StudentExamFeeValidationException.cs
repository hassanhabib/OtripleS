using System;

namespace OtripleS.Web.Api.Models.Foundations.StudentExamFees.Exceptions
{
    public class StudentExamFeeValidationException : Exception
    {
        public StudentExamFeeValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException)
        { }
    }
}
