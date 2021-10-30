// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.StudentRegistrations
{
    public partial class StudentRegistrationService
    {
        private static void ValidateStudentRegistrationOnCreate(StudentRegistration studentRegistration)
        {
            ValidateStudentRegistrationIsNull(studentRegistration);
            ValidateStudentRegistrationIds(studentRegistration.StudentId, studentRegistration.RegistrationId);
        }

        private static void ValidateStudentRegistrationIsNull(StudentRegistration studentRegistration)
        {
            if (studentRegistration is null)
            {
                throw new NullStudentRegistrationException();
            }
        }

        private static void ValidateStudentRegistrationIds(Guid studentId, Guid registrationId)
        {
            switch (studentId, registrationId)
            {
                case { } when IsInvalid(studentId):
                    throw new InvalidStudentRegistrationException(
                        parameterName: nameof(StudentRegistration.StudentId),
                        parameterValue: studentId);

                case { } when IsInvalid(registrationId):
                    throw new InvalidStudentRegistrationException(
                        parameterName: nameof(StudentRegistration.RegistrationId),
                        parameterValue: registrationId);
            }
        }

        private static bool IsInvalid(Guid input) => input == Guid.Empty;

        private static void ValidateStorageStudentRegistration(StudentRegistration storageStudentRegistration, Guid studentId, Guid registrationId)
        {
            if (storageStudentRegistration == null)
            {
                throw new NotFoundStudentRegistrationException(studentId, registrationId);
            }
        }
    }
}
