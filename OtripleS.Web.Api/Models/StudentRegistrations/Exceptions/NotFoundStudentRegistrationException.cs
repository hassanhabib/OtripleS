// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class NotFoundStudentRegistrationException : Exception
    {
        public NotFoundStudentRegistrationException(Guid studentId, Guid registrationId)
            : base(message: $"Couldn't find student registration with student id: {studentId} " +
                  $" and registration id: {registrationId}.")
        { }
    }
}

