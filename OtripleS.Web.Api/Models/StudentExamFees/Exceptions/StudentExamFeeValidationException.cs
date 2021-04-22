using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class StudentExamFeeValidationException : Exception
    {
        public StudentExamFeeValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException)
        { }
    }
}
