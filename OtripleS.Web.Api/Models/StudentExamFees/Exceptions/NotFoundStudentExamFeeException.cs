using System;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class NotFoundStudentExamFeeException : Exception
    {
        public NotFoundStudentExamFeeException(Guid studentId, Guid examFeeId)
            : base($"Couldn't find StudentExamFee with '" +
                  $"StudentId: {studentId} and " +
                  $"ExamFeeId: {examFeeId}.")
        { }
    }
}
