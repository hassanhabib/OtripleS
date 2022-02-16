﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.ExamAttachments.Exceptions
{
    public class AlreadyExistsExamAttachmentException : Exception
    {
        public AlreadyExistsExamAttachmentException(Exception innerException)
            : base(message: "Exam attachment with the same id already exists.", innerException) { }
    }
}
