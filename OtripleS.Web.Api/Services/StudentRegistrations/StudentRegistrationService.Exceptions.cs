using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
            catch (NullStudentRegistrationException nullStudentRegistrationException) 
            {
                throw CreateAndLogValidationException(nullStudentRegistrationException);
            }
            catch (InvalidStudentRegistrationException invalidStudentRegistrationException) 
            {
                throw CreateAndLogValidationException(invalidStudentRegistrationException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsStudentRegistrationException =
                    new AlreadyExistsStudentRegistrationException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsStudentRegistrationException);
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
            var studentRegistrationValidationException = new StudentRegistrationValidationException(exception);
            this.loggingBroker.LogError(studentRegistrationValidationException);

            return studentRegistrationValidationException;
        }

        private StudentRegistrationDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var studentRegistrationDependencyException = new StudentRegistrationDependencyException(exception);
            this.loggingBroker.LogCritical(studentRegistrationDependencyException);

            return studentRegistrationDependencyException;
        }

        private StudentRegistrationDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var studentRegistrationDependencyException = new StudentRegistrationDependencyException(exception);
            this.loggingBroker.LogError(studentRegistrationDependencyException);

            return studentRegistrationDependencyException;
        }

        private StudentRegistrationServiceException CreateAndLogServiceException(Exception exception)
        {
            var StudentRegistrationServiceException = new StudentRegistrationServiceException(exception);
            this.loggingBroker.LogError(StudentRegistrationServiceException);

            return StudentRegistrationServiceException;
        }
    }
}
