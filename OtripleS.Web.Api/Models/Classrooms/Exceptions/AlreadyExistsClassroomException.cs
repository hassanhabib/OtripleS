// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Classrooms.Exceptions
{
    public class AlreadyExistsClassroomException : Exception
    {
        public AlreadyExistsClassroomException(Exception innerException)
            : base("Classroom with the same id already exists.", innerException) { }
    }
}
