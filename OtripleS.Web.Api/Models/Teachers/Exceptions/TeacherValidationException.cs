// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class TeacherValidationException : Exception
    {
        public TeacherValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
