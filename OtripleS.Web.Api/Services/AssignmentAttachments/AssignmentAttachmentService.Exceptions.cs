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
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.AssignmentAttachments
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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAssignmentAttachmentException =
                    new LockedAssignmentAttachmentException(dbUpdateConcurrencyException);

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
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (NotFoundAssignmentAttachmentException notFoundAssignmentAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundAssignmentAttachmentException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
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

        private AssignmentAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var AssignmentAttachmentValidationException = new AssignmentAttachmentValidationException(exception);
            this.loggingBroker.LogError(AssignmentAttachmentValidationException);

            return AssignmentAttachmentValidationException;
        }

        private AssignmentAttachmentDependencyValidationException CreateAndLogDependencyValidationException(
            Exception exception)
        {
            var assignmentAttachmentDependencyValidationException = 
                new AssignmentAttachmentDependencyValidationException(exception);

            this.loggingBroker.LogError(assignmentAttachmentDependencyValidationException);

            return assignmentAttachmentDependencyValidationException;
        }

        private AssignmentAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var AssignmentAttachmentDependencyException = new AssignmentAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(AssignmentAttachmentDependencyException);

            return AssignmentAttachmentDependencyException;
        }

        private AssignmentAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var AssignmentAttachmentDependencyException = new AssignmentAttachmentDependencyException(exception);
            this.loggingBroker.LogError(AssignmentAttachmentDependencyException);

            return AssignmentAttachmentDependencyException;
        }

        private AssignmentAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var AssignmentAttachmentServiceException = new AssignmentAttachmentServiceException(exception);
            this.loggingBroker.LogError(AssignmentAttachmentServiceException);

            return AssignmentAttachmentServiceException;
        }
    }
}
