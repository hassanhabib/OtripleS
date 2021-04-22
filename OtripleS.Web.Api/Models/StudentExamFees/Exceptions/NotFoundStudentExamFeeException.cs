//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class NotFoundStudentExamFeeException : Exception
    {
        public NotFoundStudentExamFeeException(Guid studentExamFeeId)
            : base($"Couldn't find student's exam fee with Id: {studentExamFeeId}.") { }
    }
}
