// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class NotFoundExamException : Xeption
    {
        public NotFoundExamException(Guid examId)
            : base(message: $"Couldn't find exam with id: {examId}.") { }
    }
}
