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
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;

namespace OtripleS.Web.Api.Services.StudentContacts
{
    public partial class StudentContactService
    {
        private delegate ValueTask<StudentContact> ReturningStudentContactFunction();
        private delegate IQueryable<StudentContact> ReturningStudentContactsFunction();

        private async ValueTask<StudentContact> TryCatch(
            ReturningStudentContactFunction returningStudentContactFunction)
        {
            try
            {
                return await returningStudentContactFunction();
            }
            catch (NullStudentContactException nullStudentContactException)
            {
                throw CreateAndLogValidationException(nullStudentContactException);
            }
            catch (InvalidStudentContactInputException invalidStudentContactInputException)
            {
                throw CreateAndLogValidationException(invalidStudentContactInputException);
            }
            catch (NotFoundStudentContactException notFoundStudentContactException)
            {
                throw CreateAndLogValidationException(notFoundStudentContactException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsStudentContactException =
                    new AlreadyExistsStudentContactException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsStudentContactException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidStudentContactReferenceException =
                    new InvalidStudentContactReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidStudentContactReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedStudentContactException =
                    new LockedStudentContactException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedStudentContactException);
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

        private IQueryable<StudentContact> TryCatch(
            ReturningStudentContactsFunction returningStudentContactsFunction)
        {
            try
            {
                return returningStudentContactsFunction();
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

        private StudentContactValidationException CreateAndLogValidationException(Exception exception)
        {
            var StudentContactValidationException = new StudentContactValidationException(exception);
            this.loggingBroker.LogError(StudentContactValidationException);

            return StudentContactValidationException;
        }

        private StudentContactServiceException CreateAndLogServiceException(Exception exception)
        {
            var StudentContactServiceException = new StudentContactServiceException(exception);
            this.loggingBroker.LogError(StudentContactServiceException);

            return StudentContactServiceException;
        }

        private StudentContactDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var StudentContactDependencyException = new StudentContactDependencyException(exception);
            this.loggingBroker.LogCritical(StudentContactDependencyException);

            return StudentContactDependencyException;
        }

        private StudentContactDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var StudentContactDependencyException = new StudentContactDependencyException(exception);
            this.loggingBroker.LogError(StudentContactDependencyException);

            return StudentContactDependencyException;
        }
    }
}