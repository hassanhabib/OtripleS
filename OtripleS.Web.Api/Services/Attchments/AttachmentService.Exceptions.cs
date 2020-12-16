// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Attachments.Exceptions;

namespace OtripleS.Web.Api.Services.Attachments
{
    public partial class AttachmentService : IAttachmentService
    {
        private delegate ValueTask<Attachment> ReturningAttachmentFunction();

        private async ValueTask<Attachment> TryCatch(ReturningAttachmentFunction returningAttachmentFunction)
        {
            try
            {
                return await returningAttachmentFunction();
            }
            catch (InvalidAttachmentException invalidAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidAttachmentInputException);
            }
            catch (NotFoundAttachmentException notFoundAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundAttachmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
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