﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Models.Users.Exceptions;
using Xeptions;

namespace OtripleS.Web.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private delegate ValueTask<User> ReturningUserFunction();
        private delegate IQueryable<User> ReturningQueryableUserFunction();

        private async ValueTask<User> TryCatch(ReturningUserFunction returningUserFunction)
        {
            try
            {
                return await returningUserFunction();
            }
            catch (NullUserException nullUserException)
            {
                throw CreateAndLogValidationException(nullUserException);
            }
            catch (InvalidUserException invalidUserException)
            {
                throw CreateAndLogValidationException(invalidUserException);
            }
            catch (NotFoundUserException nullUserException)
            {
                throw CreateAndLogValidationException(nullUserException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsUserException =
                    new AlreadyExistsUserException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsUserException);
            }
            catch (SqlException sqlException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedUserStorageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedUserException =
                    new LockedUserException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedUserException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedUserStorageException);
            }
            catch (Exception exception)
            {
                var failedUserServiceException =
                    new FailedUserServiceException(exception);

                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }

        private IQueryable<User> TryCatch(ReturningQueryableUserFunction returningQueryableUserFunction)
        {
            try
            {
                return returningQueryableUserFunction();
            }
            catch (SqlException sqlException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedUserStorageException);
            }
            catch (Exception exception)
            {
                var failedUserServiceException =
                    new FailedUserServiceException(exception);

                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }

        private Exception CreateAndLogValidationException(Exception exception)
        {
            var userValidationException = new UserValidationException(exception);
            this.loggingBroker.LogError(userValidationException);

            return userValidationException;
        }

        private UserDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var userDependencyException = new UserDependencyException(exception);
            this.loggingBroker.LogError(userDependencyException);

            return userDependencyException;
        }

        private UserDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var userDependencyException = new UserDependencyException(exception);
            this.loggingBroker.LogCritical(userDependencyException);

            return userDependencyException;
        }

        private UserServiceException CreateAndLogServiceException(Exception exception)
        {
            var userServiceException = new UserServiceException(exception);
            this.loggingBroker.LogError(userServiceException);

            return userServiceException;
        }
    }
}
