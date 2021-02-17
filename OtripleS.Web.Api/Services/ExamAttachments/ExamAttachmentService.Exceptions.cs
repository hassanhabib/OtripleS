//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.ExamAttachments
{
    public partial class ExamAttachmentService
    {
        private delegate ValueTask<ExamAttachment> ReturningExamAttachmentFunction();

        private async ValueTask<ExamAttachment> TryCatch(
            ReturningExamAttachmentFunction returningExamAttachmentFunction)
        {
            try
            {
                return await returningExamAttachmentFunction();
            }
            catch (NullExamAttachmentException nullExamAttachmentException)
            {
                throw CreateAndLogValidationException(nullExamAttachmentException);
            }
            catch (InvalidExamAttachmentException invalidExamAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidExamAttachmentInputException);
            }
        }

        private ExamAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var examAttachmentValidationException = new ExamAttachmentValidationException(exception);
            this.loggingBroker.LogError(examAttachmentValidationException);

            return examAttachmentValidationException;
        }
    }
}
