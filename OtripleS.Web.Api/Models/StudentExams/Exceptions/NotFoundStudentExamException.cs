// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.StudentExams.Exceptions
{
    public class NotFoundStudentExamException : Exception
    {
        public NotFoundStudentExamException(Guid studentExamId)
            : base($"Couldn't find StudentExam with Id: {studentExamId}.") { }
    }
}
