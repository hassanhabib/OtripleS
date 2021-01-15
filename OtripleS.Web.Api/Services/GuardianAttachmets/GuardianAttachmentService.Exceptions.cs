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
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.GuardianAttachmets
{
    public partial class GuardianAttachmentService
    {
        private delegate ValueTask<GuardianAttachment> ReturningGuardianAttachmentFunction();
        private delegate IQueryable<GuardianAttachment> ReturningGuardianAttachmentsFunction();

        private async ValueTask<GuardianAttachment> TryCatch(
            ReturningGuardianAttachmentFunction returningGuardianAttachmentFunction)
        {
            try
            {
                return await returningGuardianAttachmentFunction();
            }
            catch (NullGuardianAttachmentException nullGuardianAttachmentException)
            {
                throw CreateAndLogValidationException(nullGuardianAttachmentException);
            }
            catch (InvalidGuardianAttachmentException invalidGuardianAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidGuardianAttachmentInputException);
            }
            catch (NotFoundGuardianAttachmentException notFoundGuardianAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundGuardianAttachmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsGuardianAttachmentException =
                    new AlreadyExistsGuardianAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsGuardianAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidGuardianAttachmentReferenceException =
                    new InvalidGuardianAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidGuardianAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedGuardianAttachmentException =
                    new LockedGuardianAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedGuardianAttachmentException);
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

        private IQueryable<GuardianAttachment> TryCatch(ReturningGuardianAttachmentsFunction returningGuardianAttachmentsFunction)
        {
            try
            {
                return returningGuardianAttachmentsFunction();
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

        private GuardianAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var guardianAttachmentValidationException = new GuardianAttachmentValidationException(exception);
            this.loggingBroker.LogError(guardianAttachmentValidationException);

            return guardianAttachmentValidationException;
        }

        private GuardianAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new GuardianAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private GuardianAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new GuardianAttachmentDependencyException(exception);
            this.loggingBroker.LogError(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private GuardianAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var guardianAttachmentServiceException = new GuardianAttachmentServiceException(exception);
            this.loggingBroker.LogError(guardianAttachmentServiceException);

            return guardianAttachmentServiceException;
        }
    }
}
