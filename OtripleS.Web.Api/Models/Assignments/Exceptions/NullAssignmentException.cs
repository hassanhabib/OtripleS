﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Assignments.Exceptions
{
    public class NullAssignmentException : Exception
    {
        public NullAssignmentException() : base(message: "The assignment is null.") { }
    }
}
