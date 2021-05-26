// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;

namespace OtripleS.Web.Api.Services.StudentRegistrations
{
    public partial class StudentRegistrationService
    {
        private delegate ValueTask<StudentRegistration> ReturningStudentRegistrationFunction();

        private async ValueTask<StudentRegistration> TryCatch(
            ReturningStudentRegistrationFunction returningStudentRegistrationFunction)
        {
            try
            {
                return await returningStudentRegistrationFunction();
            }
            catch (InvalidStudentRegistrationInputException invalidStudentRegistrationInputException)
            {
                throw CreateAndLogValidationException(invalidStudentRegistrationInputException);
            }
        }

        private StudentRegistrationValidationException CreateAndLogValidationException(Exception exception)
        {
            var StudentRegistrationValidationException = new StudentRegistrationValidationException(exception);
            this.loggingBroker.LogError(StudentRegistrationValidationException);

            return StudentRegistrationValidationException;
        }
    }
}
