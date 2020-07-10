// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class TeacherDependencyException : Exception
    {
        public TeacherDependencyException(Exception innerException)
            : base("Service dependency error occurred, contact support.", innerException) { }
    }
}
