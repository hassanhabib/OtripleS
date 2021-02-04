using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
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
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
        }


        private CourseAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var courseAttachmentValidationException = new CourseAttachmentValidationException(exception);
            this.loggingBroker.LogError(courseAttachmentValidationException);

            return courseAttachmentValidationException;
        }

        private CourseAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var courseAttachmentDependencyException = new CourseAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(courseAttachmentDependencyException);

            return courseAttachmentDependencyException;
        }

        private CourseAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var courseAttachmentDependencyException = new CourseAttachmentDependencyException(exception);
            this.loggingBroker.LogError(courseAttachmentDependencyException);

            return courseAttachmentDependencyException;
        }

    }
}
