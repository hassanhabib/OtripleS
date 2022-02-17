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
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using Xeptions;

namespace OtripleS.Web.Api.Services.Foundations.Classrooms
{
    public partial class ClassroomService
    {
        private delegate ValueTask<Classroom> ReturningClassroomFunction();
        private delegate IQueryable<Classroom> ReturningQueryableClassroomFunction();

        private async ValueTask<Classroom> TryCatch(ReturningClassroomFunction returningClassroomFunction)
        {
            try
            {
                return await returningClassroomFunction();
            }
            catch (NullClassroomException nullClassroomException)
            {
                throw CreateAndLogValidationException(nullClassroomException);
            }
            catch (InvalidClassroomException invalidClassroomInputException)
            {
                throw CreateAndLogValidationException(invalidClassroomInputException);
            }
            catch (NotFoundClassroomException notFoundClassroomException)
            {
                throw CreateAndLogValidationException(notFoundClassroomException);
            }
            catch (SqlException sqlException)
            {
                var failedClassroomStorageException =
                    new FailedClassroomStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedClassroomStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsClassroomException =
                    new AlreadyExistsClassroomException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsClassroomException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedClassroomException = new LockedClassroomException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedClassroomException);

            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedClassroomStorageException =
                    new FailedClassroomStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedClassroomStorageException);
            }
            catch (Exception exception)
            {
                var failedClassroomServiceException =
                    new FailedClassroomServiceException(exception);

                throw CreateAndLogServiceException(failedClassroomServiceException);
            }
        }

        private IQueryable<Classroom> TryCatch(
            ReturningQueryableClassroomFunction returningQueryableClassroomFunction)
        {
            try
            {
                return returningQueryableClassroomFunction();
            }
            catch (SqlException sqlException)
            {
                var failedClassroomStorageException = new FailedClassroomStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedClassroomStorageException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private ClassroomValidationException CreateAndLogValidationException(Exception exception)
        {
            var classroomValidationException = new ClassroomValidationException(exception);
            this.loggingBroker.LogError(classroomValidationException);

            return classroomValidationException;
        }

        private ClassroomDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var classroomDependencyException = new ClassroomDependencyException(exception);
            this.loggingBroker.LogCritical(classroomDependencyException);

            return classroomDependencyException;
        }

        private ClassroomDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var classroomDependencyException = new ClassroomDependencyException(exception);
            this.loggingBroker.LogError(classroomDependencyException);

            return classroomDependencyException;
        }

        private ClassroomDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var classroomDependencyValidationException = new ClassroomDependencyValidationException(exception);
            this.loggingBroker.LogError(classroomDependencyValidationException);

            return classroomDependencyValidationException;
        }

        private ClassroomServiceException CreateAndLogServiceException(Exception exception)
        {
            var classroomServiceException = new ClassroomServiceException(exception);
            this.loggingBroker.LogError(classroomServiceException);

            return classroomServiceException;
        }
    }
}