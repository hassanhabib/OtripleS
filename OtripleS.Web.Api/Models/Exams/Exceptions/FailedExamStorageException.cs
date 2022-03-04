// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class FailedExamStorageException : Xeption
    {
        public FailedExamStorageException(Exception innerException)
            : base(message: "Failed exam storage error occurred, please contact support.", innerException)
        { }
    }
}
