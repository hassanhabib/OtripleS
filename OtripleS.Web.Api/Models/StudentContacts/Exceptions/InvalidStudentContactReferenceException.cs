﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentContacts.Exceptions
{
    public class InvalidStudentContactReferenceException : Exception
    {
        public InvalidStudentContactReferenceException(Exception innerException)
            : base(message: "Invalid student contact reference error occurred.", innerException) { }
    }
}
