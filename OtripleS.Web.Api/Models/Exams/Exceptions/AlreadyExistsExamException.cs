// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class AlreadyExistsExamException : Exception
    {
        public AlreadyExistsExamException(Exception innerException)
            : base("Exam with the same id already exists.", innerException) { }
    }
}
