// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;

namespace OtripleS.Web.Api.Services.StudentGuardians
{
	public partial class StudentGuardianService
	{
        private delegate ValueTask<StudentGuardian> ReturningStudentGuardianFunction();

        private async ValueTask<StudentGuardian> TryCatch(
           ReturningStudentGuardianFunction returningStudentGuardianFunction)
        {
            try
            {
                return await returningStudentGuardianFunction();
            }
            catch (NullStudentGuardianException nullStudentGuardianException)
            {
                throw CreateAndLogValidationException(nullStudentGuardianException);
            }
            catch (InvalidStudentGuardianInputException invalidStudentGuardianInputException)
            {
                throw CreateAndLogValidationException(invalidStudentGuardianInputException);
            }
            catch (NotFoundStudentGuardianException notFoundStudentGuardianException)
            {
                throw CreateAndLogValidationException(notFoundStudentGuardianException);
            }
        }

        private StudentGuardianValidationException CreateAndLogValidationException(Exception exception)
        {
            var StudentGuardianValidationException = new StudentGuardianValidationException(exception);
            this.loggingBroker.LogError(StudentGuardianValidationException);

            return StudentGuardianValidationException;
        }
    }
}
