// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            catch (NotFoundStudentRegistrationException nullStudentRegistrationException)
            {
                throw CreateAndLogValidationException(nullStudentRegistrationException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private StudentRegistrationValidationException CreateAndLogValidationException(Exception exception)
        {
            var StudentRegistrationValidationException = new StudentRegistrationValidationException(exception);
            this.loggingBroker.LogError(StudentRegistrationValidationException);

            return StudentRegistrationValidationException;
        }

        private StudentRegistrationDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var StudentRegistrationDependencyException = new StudentRegistrationDependencyException(exception);
            this.loggingBroker.LogCritical(StudentRegistrationDependencyException);

            return StudentRegistrationDependencyException;
        }

        private StudentRegistrationDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var StudentRegistrationDependencyException = new StudentRegistrationDependencyException(exception);
            this.loggingBroker.LogError(StudentRegistrationDependencyException);

            return StudentRegistrationDependencyException;
        }

        private StudentRegistrationServiceException CreateAndLogServiceException(Exception exception)
        {
            var studentRegistrationServiceException = new StudentRegistrationServiceException(exception);
            this.loggingBroker.LogError(studentRegistrationServiceException);

            return studentRegistrationServiceException;
        }
    }
}
