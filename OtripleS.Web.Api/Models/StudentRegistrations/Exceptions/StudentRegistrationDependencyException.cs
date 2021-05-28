// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class StudentRegistrationDependencyException : Exception
    {
        public StudentRegistrationDependencyException(Exception innerException) : base(
            "Service dependency error occurred, contact support.", innerException)
        {
        }
    }
}
