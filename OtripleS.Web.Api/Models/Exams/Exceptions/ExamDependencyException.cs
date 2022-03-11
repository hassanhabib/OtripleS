// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class ExamDependencyException : Xeption
    {
        public ExamDependencyException(Xeption innerException)
             : base(message: "Exam dependency error occurred, contact support.", innerException) { }
    }
}
