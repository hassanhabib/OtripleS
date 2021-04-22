using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class LockedStudentExamFeeException : Exception
    {
        public LockedStudentExamFeeException(Exception innerException)
          : base("Locked Assignment Attachment record exception, please try again later.", innerException) { }
    }
}
