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
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;
using Xeptions;

namespace OtripleS.Web.Api.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentService
    {
        private delegate ValueTask<AssignmentAttachment> ReturningAssignmentAttachmentFunction();
        private delegate IQueryable<AssignmentAttachment> ReturningAssignmentAttachmentsFunction();

        private async ValueTask<AssignmentAttachment> TryCatch(
            ReturningAssignmentAttachmentFunction returningAssignmentAttachmentFunction)
        {
            try
            {
                return await returningAssignmentAttachmentFunction();
            }
            catch (NullAssignmentAttachmentException nullAssignmentAttachmentInputException)
            {
                throw CreateAndLogValidationException(nullAssignmentAttachmentInputException);
            }
            catch (InvalidAssignmentAttachmentException invalidAssignmentAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidAssignmentAttachmentInputException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAssignmentAttachmentException =
                    new AlreadyExistsAssignmentAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsAssignmentAttachmentException);
            }
            catch (DbUpdateConcurrencyException databaseUpdateConcurrencyException)
            {
                var lockedAssignmentAttachmentException =
                    new LockedAssignmentAttachmentException(databaseUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedAssignmentAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAssignmentAttachmentReferenceException =
                    new InvalidAssignmentAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidAssignmentAttachmentReferenceException);
            }
            catch (SqlException sqlException)
            {
                var failedAssigmentAttachmentStorageException =
                    new FailedAssignmentAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAssigmentAttachmentStorageException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAssigmentAttachmentStorageException =
                    new FailedAssignmentAttachmentStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedAssigmentAttachmentStorageException);

            }
            catch (NotFoundAssignmentAttachmentException notFoundAssignmentAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundAssignmentAttachmentException);
            }
            catch (Exception exception)
            {
                var failedAssignmentAttachmentServiceException =
                    new FailedAssignmentAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedAssignmentAttachmentServiceException);
            }
        }

        private IQueryable<AssignmentAttachment> TryCatch(
            ReturningAssignmentAttachmentsFunction returningAssignmentAttachmentsFunction)
        {
            try
            {
                return returningAssignmentAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAssigmentAttachmentStorageException =
                    new FailedAssignmentAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAssigmentAttachmentStorageException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }

        }

        private AssignmentAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var assignmentAttachmentValidationException = new AssignmentAttachmentValidationException(exception);
            this.loggingBroker.LogError(assignmentAttachmentValidationException);

            return assignmentAttachmentValidationException;
        }

        private AssignmentAttachmentDependencyValidationException CreateAndLogDependencyValidationException(
            Exception exception)
        {
            var assignmentAttachmentDependencyValidationException =
                new AssignmentAttachmentDependencyValidationException(exception);

            this.loggingBroker.LogError(assignmentAttachmentDependencyValidationException);

            return assignmentAttachmentDependencyValidationException;
        }

        private AssignmentAttachmentDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var assignmentAttachmentDependencyException =
                new AssignmentAttachmentDependencyException(exception);

            this.loggingBroker.LogCritical(assignmentAttachmentDependencyException);

            return assignmentAttachmentDependencyException;
        }

        private AssignmentAttachmentDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var assignmentAttachmentDependencyException = new AssignmentAttachmentDependencyException(exception);
            this.loggingBroker.LogError(assignmentAttachmentDependencyException);

            return assignmentAttachmentDependencyException;
        }

        private AssignmentAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var assignmentAttachmentServiceException = new AssignmentAttachmentServiceException(exception);
            this.loggingBroker.LogError(assignmentAttachmentServiceException);

            return assignmentAttachmentServiceException;
        }
    }
}
