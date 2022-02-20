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
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;
using Xeptions;

namespace OtripleS.Web.Api.Services.Foundations.Teachers
{
    public partial class TeacherService
    {
        private delegate ValueTask<Teacher> ReturningTeacherFunction();
        private delegate IQueryable<Teacher> ReturningQueryableTeacherFunction();

        private async ValueTask<Teacher> TryCatch(ReturningTeacherFunction returningTeacherFunction)
        {
            try
            {
                return await returningTeacherFunction();
            }
            catch (NullTeacherException nullTeacherException)
            {
                throw CreateAndLogValidationException(nullTeacherException);
            }
            catch (InvalidTeacherException invalidTeacherException)
            {
                throw CreateAndLogValidationException(invalidTeacherException);
            }
            catch (NotFoundTeacherException notFoundTeacherException)
            {
                throw CreateAndLogValidationException(notFoundTeacherException);
            }
            catch (SqlException sqlException)
            {
                var failedTeacherStorageExceptin =
                    new FailedTeacherStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTeacherStorageExceptin);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {

                var alreadyExistsTeacherException =
                    new AlreadyExistsTeacherException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsTeacherException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTeacherException = new LockedTeacherException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedTeacherException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedTeacherStorageException =
                    new FailedTeacherStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedTeacherStorageException);
            }
            catch (Exception exception)
            {
                var failedTeacherServiceException =
                    new FailedTeacherServiceException(exception);

                throw CreateAndLogServiceException(failedTeacherServiceException);
            }
        }

        private IQueryable<Teacher> TryCatch(ReturningQueryableTeacherFunction returningQueryableTeacherFunction)
        {
            try
            {
                return returningQueryableTeacherFunction();
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTeacherException = new LockedTeacherException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedTeacherException);
            }
            catch (Exception exception)
            {
                var failedTeacherServiceException =
                    new FailedTeacherServiceException(exception);

                throw CreateAndLogServiceException(failedTeacherServiceException);
            }
        }

        private Exception CreateAndLogDependencyValidationException(Xeption exception)
        {
            var teacherDependencyValidationException =
                new TeacherDependencyValidationException(exception);

            this.loggingBroker.LogError(teacherDependencyValidationException);

            return teacherDependencyValidationException;
        }

        private TeacherValidationException CreateAndLogValidationException(Xeption exception)
        {
            var teacherValidationException = new TeacherValidationException(exception);
            this.loggingBroker.LogError(teacherValidationException);

            return teacherValidationException;
        }   
        private TeacherValidationException CreateAndLogValidationException(Exception exception)
        {
            var teacherValidationException = new TeacherValidationException(exception);
            this.loggingBroker.LogError(teacherValidationException);

            return teacherValidationException;
        }
        private TeacherDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var teacherDependencyException = new TeacherDependencyException(exception);
            this.loggingBroker.LogCritical(teacherDependencyException);

            return teacherDependencyException;
        }

        private TeacherDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var teacherDependencyException = new TeacherDependencyException(exception);
            this.loggingBroker.LogError(teacherDependencyException);

            return teacherDependencyException;
        }

        private TeacherServiceException CreateAndLogServiceException(Exception exception)
        {
            var teacherServiceException = new TeacherServiceException(exception);
            this.loggingBroker.LogError(teacherServiceException);

            return teacherServiceException;
        }
    }
}
