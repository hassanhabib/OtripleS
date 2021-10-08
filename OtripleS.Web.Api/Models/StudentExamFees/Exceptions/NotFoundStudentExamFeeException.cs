// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class NotFoundStudentExamFeeException : Exception
    {
        public NotFoundStudentExamFeeException(Guid studentId, Guid examFeeId)
            : base(message: $"Couldn't find student exam fee with '" +
                  $"student id: {studentId} and " +
                  $"exam fee id: {examFeeId}.")
        { }
    }
}
