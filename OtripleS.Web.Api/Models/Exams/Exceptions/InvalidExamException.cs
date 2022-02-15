// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class InvalidExamException : Xeption
    {
        public InvalidExamException()
            : base(message: "Invalid exam. Please fix the errors and try again.")
        { }
    }
}