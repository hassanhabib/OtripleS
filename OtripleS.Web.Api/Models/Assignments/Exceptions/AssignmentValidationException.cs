// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Assignments.Exceptions
{
    public class AssignmentValidationException : Xeption
    {
        public AssignmentValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
    }
}
