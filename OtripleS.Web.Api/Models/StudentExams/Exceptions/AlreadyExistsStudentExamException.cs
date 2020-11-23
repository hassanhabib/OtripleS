// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentExams.Exceptions
{
    public class AlreadyExistsStudentExamException : Exception
    {
        public AlreadyExistsStudentExamException(Exception innerException)
            : base("Student exam with the same id already exists.", innerException) { }
    }
}
