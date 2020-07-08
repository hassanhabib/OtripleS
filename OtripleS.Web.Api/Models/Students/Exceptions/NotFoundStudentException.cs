// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class NotFoundStudentException : Exception
    {
        public NotFoundStudentException(Guid studentId)
            : base($"Couldn't find student with Id: {studentId}.") { }
    }
}
