// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.Foundations.StudentRegistrations.Exceptions
{
    public class NotFoundStudentRegistrationException : Exception
    {
        public NotFoundStudentRegistrationException(Guid studentId, Guid registrationId)
            : base($"Couldn't find StudentRegistration with Student Id: {studentId} " +
                  $" and Registration Id: {registrationId}.") { }
    }
}

