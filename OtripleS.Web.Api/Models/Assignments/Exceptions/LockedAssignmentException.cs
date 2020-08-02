// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Assignments.Exceptions
{
    public class LockedAssignmentException : Exception
    {
        public LockedAssignmentException(Exception innerException)
            : base("Locked assignment record exception, please try again later.", innerException) { }
    }
}
