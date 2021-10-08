// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class InvalidStudentRegistrationReferenceException : Exception
    {
        public InvalidStudentRegistrationReferenceException(Exception innerException)
            : base(message: "Invalid student registration reference error occurred.", innerException) { }
    }
}
