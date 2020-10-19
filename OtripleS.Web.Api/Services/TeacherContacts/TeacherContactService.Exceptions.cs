//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;

namespace OtripleS.Web.Api.Services.TeacherContacts
{
    public partial class TeacherContactService
    {
        private delegate ValueTask<TeacherContact> ReturningTeacherContactFunction();
        private delegate IQueryable<TeacherContact> ReturningTeacherContactsFunction();

        private async ValueTask<TeacherContact> TryCatch(
            ReturningTeacherContactFunction returningTeacherContactFunction)
        {
            try
            {
                return await returningTeacherContactFunction();
            }
            catch (NullTeacherContactException nullTeacherContactException)
            {
                throw CreateAndLogValidationException(nullTeacherContactException);
            }
            catch (InvalidTeacherContactInputException invalidTeacherContactInputException)
            {
                throw CreateAndLogValidationException(invalidTeacherContactInputException);
            }
            catch (NotFoundTeacherContactException notFoundTeacherContactException)
            {
                throw CreateAndLogValidationException(notFoundTeacherContactException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTeacherContactException =
                    new AlreadyExistsTeacherContactException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsTeacherContactException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidTeacherContactReferenceException =
                    new InvalidTeacherContactReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidTeacherContactReferenceException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTeacherContactException =
                    new LockedTeacherContactException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedTeacherContactException);
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

        private IQueryable<TeacherContact> TryCatch(
            ReturningTeacherContactsFunction returningTeacherContactsFunction)
        {
            try
            {
                return returningTeacherContactsFunction();
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

        private TeacherContactValidationException CreateAndLogValidationException(Exception exception)
        {
            var teacherContactValidationException = new TeacherContactValidationException(exception);
            this.loggingBroker.LogError(teacherContactValidationException);

            return teacherContactValidationException;
        }
        private TeacherContactDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var teacherContactDependencyException = new TeacherContactDependencyException(exception);
            this.loggingBroker.LogCritical(teacherContactDependencyException);

            return teacherContactDependencyException;
        }

        private TeacherContactDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var TeacherContactDependencyException = new TeacherContactDependencyException(exception);
            this.loggingBroker.LogError(TeacherContactDependencyException);

            return TeacherContactDependencyException;
        }

        private TeacherContactServiceException CreateAndLogServiceException(Exception exception)
        {
            var teacherContactServiceException = new TeacherContactServiceException(exception);
            this.loggingBroker.LogError(teacherContactServiceException);

            return teacherContactServiceException;
        }
    }
}