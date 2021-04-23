using System;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class NotFoundStudentExamFeeException : Exception
    {
        public NotFoundStudentExamFeeException(Guid studentExamFeeId)
            : base($"Couldn't find student's exam fee with Id: {studentExamFeeId}.") { }
    }
}
