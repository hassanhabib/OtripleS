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
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.GuardianContacts
{
    public partial class GuardianContactService
    {
        private delegate ValueTask<GuardianContact> ReturningGuardianContactFunction();
        private delegate IQueryable<GuardianContact> ReturningGuardianContactsFunction();

        private async ValueTask<GuardianContact> TryCatch(
            ReturningGuardianContactFunction returningGuardianContactFunction)
        {
            try
            {
                return await returningGuardianContactFunction();
            }
            catch (NullGuardianContactException nullGuardianContactException)
            {
                throw CreateAndLogValidationException(nullGuardianContactException);
            }
            catch (InvalidGuardianContactException invalidGuardianContactException)
            {
                throw CreateAndLogValidationException(invalidGuardianContactException);
            }
            catch (NotFoundGuardianContactException notFoundGuardianContactException)
            {
                throw CreateAndLogValidationException(notFoundGuardianContactException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsGuardianContactException =
                    new AlreadyExistsGuardianContactException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsGuardianContactException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidGuardianContactReferenceException =
                    new InvalidGuardianContactReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidGuardianContactReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedGuardianContactException =
                    new LockedGuardianContactException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedGuardianContactException);
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
                var failedGuardianContactServiceException =
                    new FailedGuardianContactServiceException(exception);

                throw CreateAndLogServiceException(failedGuardianContactServiceException);
            }
        }

        private IQueryable<GuardianContact> TryCatch(
            ReturningGuardianContactsFunction returningGuardianContactsFunction)
        {
            try
            {
                return returningGuardianContactsFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                var failedGuardianContactServiceException =
                    new FailedGuardianContactServiceException(exception);

                throw CreateAndLogServiceException(failedGuardianContactServiceException);
            }
        }

        private GuardianContactValidationException CreateAndLogValidationException(Exception exception)
        {
            var guardianContactValidationException = new GuardianContactValidationException(exception);
            this.loggingBroker.LogError(guardianContactValidationException);

            return guardianContactValidationException;
        }

        private GuardianContactDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var guardianContactDependencyException = new GuardianContactDependencyException(exception);
            this.loggingBroker.LogCritical(guardianContactDependencyException);

            return guardianContactDependencyException;
        }

        private GuardianContactDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var guardianContactDependencyException = new GuardianContactDependencyException(exception);
            this.loggingBroker.LogError(guardianContactDependencyException);

            return guardianContactDependencyException;
        }
        private GuardianContactServiceException CreateAndLogServiceException(Exception exception)
        {
            var guardianContactServiceException = new GuardianContactServiceException(exception);
            this.loggingBroker.LogError(guardianContactServiceException);

            return guardianContactServiceException;
        }
    }
}