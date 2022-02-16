﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class ExamServiceException : Exception
    {
        public ExamServiceException(Exception innerException)
            : base(message: "Service error occurred, contact support.", innerException) { }
    }
}