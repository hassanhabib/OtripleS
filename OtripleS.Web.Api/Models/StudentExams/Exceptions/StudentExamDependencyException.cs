﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentExams.Exceptions
{
    public class StudentExamDependencyException : Exception
    {
        public StudentExamDependencyException(Exception innerException)
            : base(message: "Service dependency error occurred, contact support.", innerException) { }
    }
}
