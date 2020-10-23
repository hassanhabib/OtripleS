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
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;

namespace OtripleS.Web.Api.Services.Classrooms
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
            catch (InvalidClassroomInputException invalidClassroomInputException)
            {
                throw CreateAndLogValidationException(invalidClassroomInputException);
            }
            catch (NotFoundClassroomException notFoundClassroomException)
            {
                throw CreateAndLogValidationException(notFoundClassroomException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
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

                throw CreateAndLogDependencyException(lockedClassroomException);
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

        private IQueryable<Classroom> TryCatch(
            ReturningQueryableClassroomFunction returningQueryableClassroomFunction)
        {
            try
            {
                return returningQueryableClassroomFunction();
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

        private ClassroomValidationException CreateAndLogValidationException(Exception exception)
        {
            var classroomValidationException = new ClassroomValidationException(exception);
            this.loggingBroker.LogError(classroomValidationException);

            return classroomValidationException;
        }

        private ClassroomDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var classroomDependencyException = new ClassroomDependencyException(exception);
            this.loggingBroker.LogCritical(classroomDependencyException);

            return classroomDependencyException;
        }

        private ClassroomDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var classroomDependencyException = new ClassroomDependencyException(exception);
            this.loggingBroker.LogError(classroomDependencyException);

            return classroomDependencyException;
        }

        private ClassroomServiceException CreateAndLogServiceException(Exception exception)
        {
            var classroomServiceException = new ClassroomServiceException(exception);
            this.loggingBroker.LogError(classroomServiceException);

            return classroomServiceException;
        }
    }
}
