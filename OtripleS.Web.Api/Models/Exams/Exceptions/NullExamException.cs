﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class NullExamException : Xeption
    {
        public NullExamException() : base(message: "The exam is null.") { }
    }
}
