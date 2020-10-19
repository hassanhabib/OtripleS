// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;

namespace OtripleS.Web.Api.Services.StudentGuardians
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
            catch (InvalidStudentGuardianInputException invalidStudentGuardianInputException)
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
                throw CreateAndLogServiceException(exception);
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
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private StudentGuardianValidationException CreateAndLogValidationException(Exception exception)
        {
            var StudentGuardianValidationException = new StudentGuardianValidationException(exception);
            this.loggingBroker.LogError(StudentGuardianValidationException);

            return StudentGuardianValidationException;
        }

        private StudentGuardianDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var StudentGuardianDependencyException = new StudentGuardianDependencyException(exception);
            this.loggingBroker.LogCritical(StudentGuardianDependencyException);

            return StudentGuardianDependencyException;
        }

        private StudentGuardianDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var StudentGuardianDependencyException = new StudentGuardianDependencyException(exception);
            this.loggingBroker.LogError(StudentGuardianDependencyException);

            return StudentGuardianDependencyException;
        }

        private StudentGuardianServiceException CreateAndLogServiceException(Exception exception)
        {
            var StudentGuardianServiceException = new StudentGuardianServiceException(exception);
            this.loggingBroker.LogError(StudentGuardianServiceException);

            return StudentGuardianServiceException;
        }
    }
}
