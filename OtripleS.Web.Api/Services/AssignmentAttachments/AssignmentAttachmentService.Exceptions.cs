//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.AssignmentAttachments
{
    public partial class AssignmentAttachmentService
    {
        private delegate ValueTask<AssignmentAttachment> ReturningAssignmentAttachmentFunction();

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
        }

        private AssignmentAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var AssignmentAttachmentValidationException = new AssignmentAttachmentValidationException(exception);
            this.loggingBroker.LogError(AssignmentAttachmentValidationException);

            return AssignmentAttachmentValidationException;
        }

        private AssignmentAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var AssignmentAttachmentDependencyException = new AssignmentAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(AssignmentAttachmentDependencyException);

            return AssignmentAttachmentDependencyException;
        }
    }
}
