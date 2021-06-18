// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Foundations.Attachments;
using OtripleS.Web.Api.Models.Foundations.Attachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Attachments
{
    public partial class AttachmentService : IAttachmentService
    {
        private delegate ValueTask<Attachment> ReturningAttachmentFunction();
        private delegate IQueryable<Attachment> ReturningAttachmentsFunction();

        private async ValueTask<Attachment> TryCatch(ReturningAttachmentFunction returningAttachmentFunction)
        {
            try
            {
                return await returningAttachmentFunction();
            }
            catch (NullAttachmentException nullAttachmentException)
            {
                throw CreateAndLogValidationException(nullAttachmentException);
            }
            catch (InvalidAttachmentException invalidAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidAttachmentInputException);
            }
            catch (NotFoundAttachmentException notFoundAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundAttachmentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAttachmentException =
                    new AlreadyExistsAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsAttachmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAttachmentException = new LockedAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedAttachmentException);
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

        private IQueryable<Attachment> TryCatch(ReturningAttachmentsFunction returningAttachmentsFunction)
        {
            try
            {
                return returningAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private AttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var attachmentValidationException = new AttachmentValidationException(exception);
            this.loggingBroker.LogError(attachmentValidationException);

            return attachmentValidationException;
        }

        private AttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var attachmentDependencyException = new AttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(attachmentDependencyException);

            return attachmentDependencyException;
        }

        private AttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var attachmentDependencyException = new AttachmentDependencyException(exception);
            this.loggingBroker.LogError(attachmentDependencyException);

            return attachmentDependencyException;
        }

        private AttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var attachmentServiceException = new AttachmentServiceException(exception);
            this.loggingBroker.LogError(attachmentServiceException);

            return attachmentServiceException;
        }
    }
}