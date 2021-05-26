// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;

namespace OtripleS.Web.Api.Services.StudentRegistrations
{
    public partial class StudentRegistrationService
    {
        private void ValidateStudentRegistrationId(Guid studentId, Guid registrationId)
        {
            if (studentId == Guid.Empty)
            {
                throw new InvalidStudentRegistrationInputException(
                    parameterName: nameof(StudentRegistration.StudentId),
                    parameterValue: studentId);
            }
        }
    }
}
