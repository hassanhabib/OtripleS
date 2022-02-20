// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class FailedTeacherStorageException : Xeption
    {
        public FailedTeacherStorageException(Exception innerException)
            : base(message: "Failed teacher storage error occurred, contact support.", innerException)
        { }
    }
}
