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
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;

namespace OtripleS.Web.Api.Services.Teachers
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
            catch (InvalidTeacherInputException invalidTeacherInputException)
            {
                throw CreateAndLogValidationException(invalidTeacherInputException);
            }
            catch (NotFoundTeacherException notFoundTeacherException)
            {
                throw CreateAndLogValidationException(notFoundTeacherException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTeacherException =
                    new AlreadyExistsTeacherException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsTeacherException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTeacherException = new LockedTeacherException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedTeacherException);
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

        private IQueryable<Teacher> TryCatch(ReturningQueryableTeacherFunction returningQueryableTeacherFunction)
        {
            try
            {
                return returningQueryableTeacherFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTeacherException = new LockedTeacherException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedTeacherException);
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

        private TeacherServiceException CreateAndLogServiceException(Exception exception)
        {
            var teacherServiceException = new TeacherServiceException(exception);
            this.loggingBroker.LogError(teacherServiceException);

            return teacherServiceException;
        }

        private TeacherDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var teacherDependencyException = new TeacherDependencyException(exception);
            this.loggingBroker.LogError(teacherDependencyException);

            return teacherDependencyException;
        }

        private TeacherDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var teacherDependencyException = new TeacherDependencyException(exception);
            this.loggingBroker.LogCritical(teacherDependencyException);

            return teacherDependencyException;
        }

        private TeacherValidationException CreateAndLogValidationException(Exception exception)
        {
            var teacherValidationException = new TeacherValidationException(exception);
            this.loggingBroker.LogError(teacherValidationException);

            return teacherValidationException;
        }
    }
}
