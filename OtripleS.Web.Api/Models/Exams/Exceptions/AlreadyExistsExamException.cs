// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class AlreadyExistsExamException : Xeption
    {
        public AlreadyExistsExamException(Exception innerException)
            : base(message: "Exam with the same id already exists.", innerException) { }
    }
}
