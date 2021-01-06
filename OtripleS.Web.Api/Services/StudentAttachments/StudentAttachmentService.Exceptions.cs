//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentAttachments;
using OtripleS.Web.Api.Models.StudentAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.StudentAttachments
{
    public partial class StudentAttachmentService
    {
        private delegate ValueTask<StudentAttachment> ReturningStudentAttachmentFunction();

        private async ValueTask<StudentAttachment> TryCatch(
            ReturningStudentAttachmentFunction returningStudentAttachmentFunction)
        {
            try
            {
                return await returningStudentAttachmentFunction();
            }
            catch (InvalidStudentAttachmentException invalidStudentAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidStudentAttachmentInputException);
            }
            catch (NotFoundStudentAttachmentException notFoundStudentAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundStudentAttachmentException);
            }
        }

        private StudentAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var StudentAttachmentValidationException = new StudentAttachmentValidationException(exception);
            this.loggingBroker.LogError(StudentAttachmentValidationException);

            return StudentAttachmentValidationException;
        }
    }
}
