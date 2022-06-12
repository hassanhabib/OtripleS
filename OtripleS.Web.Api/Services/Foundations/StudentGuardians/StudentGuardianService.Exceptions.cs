// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.StudentGuardians
{
    public partial class StudentGuardianService
    {
        private delegate ValueTask<StudentGuardian> ReturningStudentGuardianFunction();
        private delegate IQueryable<StudentGuardian> ReturningStudentGuardiansFunction();

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
            catch (InvalidStudentGuardiantException invalidStudentGuardianInputException)
            {
                throw CreateAndLogValidationException(invalidStudentGuardianInputException);
            }
            catch (NotFoundStudentGuardianException notFoundStudentGuardianException)
            {
                throw CreateAndLogValidationException(notFoundStudentGuardianException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsGuardianException =
                    new AlreadyExistsStudentGuardianException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsGuardianException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSemesterCourseException =
                    new LockedStudentGuardianException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedSemesterCourseException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                var failedStudentGuardianServiceException =
                    new FailedStudentGuardianServiceException(exception);

                throw CreateAndLogServiceException(failedStudentGuardianServiceException);
            }
        }

        private IQueryable<StudentGuardian> TryCatch(ReturningStudentGuardiansFunction returningStudentGuardiansFunction)
        {
            try
            {
                return returningStudentGuardiansFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                var failedStudentGuardianServiceException =
                    new FailedStudentGuardianServiceException(exception);

                throw CreateAndLogServiceException(failedStudentGuardianServiceException);
            }
        }

        private StudentGuardianValidationException CreateAndLogValidationException(Exception exception)
        {
            var studentGuardianValidationException = new StudentGuardianValidationException(exception);
            this.loggingBroker.LogError(studentGuardianValidationException);

            return studentGuardianValidationException;
        }

        private StudentGuardianDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var studentGuardianDependencyException = new StudentGuardianDependencyException(exception);
            this.loggingBroker.LogCritical(studentGuardianDependencyException);

            return studentGuardianDependencyException;
        }

        private StudentGuardianDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var studentGuardianDependencyException = new StudentGuardianDependencyException(exception);
            this.loggingBroker.LogError(studentGuardianDependencyException);

            return studentGuardianDependencyException;
        }

        private StudentGuardianServiceException CreateAndLogServiceException(Exception exception)
        {
            var studentGuardianServiceException = new StudentGuardianServiceException(exception);
            this.loggingBroker.LogError(studentGuardianServiceException);

            return studentGuardianServiceException;
        }
    }
}
