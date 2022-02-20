// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class TeacherDependencyValidationException : Xeption
    {
        public TeacherDependencyValidationException(Xeption innerException)
           : base(message: "Teacher dependency validation occurred,fix the errors and try again.", innerException)
        { }
    }
}
