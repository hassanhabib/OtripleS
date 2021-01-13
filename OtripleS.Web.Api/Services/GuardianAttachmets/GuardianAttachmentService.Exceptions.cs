//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.GuardianAttachmets
{
    public partial class GuardianAttachmentService
    {
        private delegate ValueTask<GuardianAttachment> ReturningGuardianAttachmentFunction();

        private async ValueTask<GuardianAttachment> TryCatch(
            ReturningGuardianAttachmentFunction returningGuardianAttachmentFunction)
        {
            try
            {
                return await returningGuardianAttachmentFunction();
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
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
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
    }
}
