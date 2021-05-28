// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;

namespace OtripleS.Web.Api.Services.StudentRegistrations
{
    public partial class StudentRegistrationService
    {
        private void ValidateStorageStudentRegistrations(IQueryable<StudentRegistration> storageStudentRegistrations)
        {
            if (storageStudentRegistrations.Count() == 0)
            {
                this.loggingBroker.LogWarning("No studentRegistrations found in storage.");
            }
        }
        private void ValidateStudentRegistrationId(Guid studentId, Guid registrationId)
        {
            if (studentId == Guid.Empty)
            {
                throw new InvalidStudentRegistrationInputException(
                    parameterName: nameof(StudentRegistration.StudentId),
                    parameterValue: studentId);
            }
        }
        private static void ValidateStorageStudentRegistration(StudentRegistration storageStudentRegistration, Guid studentId, Guid registrationId)
        {
            if (storageStudentRegistration == null)
            {
                throw new NotFoundStudentRegistrationException(studentId,registrationId);
            }
        }
    }
}
