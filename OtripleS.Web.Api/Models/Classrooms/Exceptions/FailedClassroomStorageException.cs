// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Classrooms.Exceptions
{
    public class FailedClassroomStorageException : Xeption
    {
        public FailedClassroomStorageException(Exception innerException)
            : base(message: "Failed classroom storage error occurred, contact support.", innerException)
        { }
    }
}