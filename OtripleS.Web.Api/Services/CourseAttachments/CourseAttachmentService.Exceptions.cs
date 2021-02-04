using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.CourseAttachments
{
    public partial class CourseAttachmentService
    {
        private delegate ValueTask<CourseAttachment> ReturningCourseAttachmentFunction();

        private async ValueTask<CourseAttachment> TryCatch(
            ReturningCourseAttachmentFunction returningCourseAttachmentFunction)
        {
            try
            {
                return await returningCourseAttachmentFunction();
            }
            catch (NullCourseAttachmentException nullCourseAttachmentException)
            {
                throw CreateAndLogValidationException(nullCourseAttachmentException);
            }
            catch (InvalidCourseAttachmentException invalidCourseAttachmentException)
            {
                throw CreateAndLogValidationException(invalidCourseAttachmentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCourseAttachmentException =
                    new AlreadyExistsCourseAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCourseAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidCourseAttachmentReferenceException =
                    new InvalidCourseAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidCourseAttachmentReferenceException);
            }
        }


        private CourseAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var courseAttachmentValidationException = new CourseAttachmentValidationException(exception);
            this.loggingBroker.LogError(courseAttachmentValidationException);

            return courseAttachmentValidationException;
        }

    }
}
