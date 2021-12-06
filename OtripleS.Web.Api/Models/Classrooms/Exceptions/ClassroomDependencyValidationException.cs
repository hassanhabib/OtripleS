// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Classrooms.Exceptions
{
    public class ClassroomDependencyValidationException : Xeption
    {
        public ClassroomDependencyValidationException(Xeption innerException)
            : base(message: "Classroom dependency validation occurred, fix the errors and try again.", innerException)
        { }
    }
}