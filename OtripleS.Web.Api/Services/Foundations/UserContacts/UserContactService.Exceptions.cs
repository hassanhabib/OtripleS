// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.UserContacts
{
    public partial class UserContactService
    {
        private delegate ValueTask<UserContact> ReturningUserContactFunction();
        private delegate IQueryable<UserContact> ReturningUserContactsFunction();

        private async ValueTask<UserContact> TryCatch(ReturningUserContactFunction returningUserContactFunction)
        {
            try
            {
                return await returningUserContactFunction();
            }
            catch (NullUserContactException nullUserContactException)
            {
                throw CreateAndLogValidationException(nullUserContactException);
            }
            catch (InvalidUserContactException invalidUserContactException)
            {
                throw CreateAndLogValidationException(invalidUserContactException);
            }
            catch (SqlException sqlException)
            {
                var failedUserContactStorageException =
                    new FailedUserContactStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedUserContactStorageException);
            }
            catch (NotFoundUserContactException notFoundUserContactException)
            {
                throw CreateAndLogValidationException(notFoundUserContactException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsUserContactException =
                    new AlreadyExistsUserContactException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsUserContactException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidUserContactReferenceException =
                    new InvalidUserContactReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidUserContactReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedUserContactException =
                    new LockedUserContactException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedUserContactException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedUserContactStorageException =
                    new FailedUserContactStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedUserContactStorageException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private IQueryable<UserContact> TryCatch(
           ReturningUserContactsFunction returningUserContactsFunction)
        {
            try
            {
                return returningUserContactsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedUserContactStorageException =
                    new FailedUserContactStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedUserContactStorageException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private UserContactValidationException CreateAndLogValidationException(Exception exception)
        {
            var UserContactValidationException = new UserContactValidationException(exception);
            this.loggingBroker.LogError(UserContactValidationException);

            return UserContactValidationException;
        }

        private UserContactDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var UserContactDependencyException = new UserContactDependencyException(exception);
            this.loggingBroker.LogCritical(UserContactDependencyException);

            return UserContactDependencyException;
        }

        private UserContactDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var UserContactDependencyException = new UserContactDependencyException(exception);
            this.loggingBroker.LogError(UserContactDependencyException);

            return UserContactDependencyException;
        }

        private UserContactServiceException CreateAndLogServiceException(Exception exception)
        {
            var UserContactServiceException = new UserContactServiceException(exception);
            this.loggingBroker.LogError(UserContactServiceException);

            return UserContactServiceException;
        }
    }
}
