//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        }

        private GuardianAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var guardianAttachmentValidationException = new GuardianAttachmentValidationException(exception);
            this.loggingBroker.LogError(guardianAttachmentValidationException);

            return guardianAttachmentValidationException;
        }
    }
}
