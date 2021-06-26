// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class NullStudentException : Exception
    {
        public NullStudentException() : base("The student is null.") { }
    }
}
