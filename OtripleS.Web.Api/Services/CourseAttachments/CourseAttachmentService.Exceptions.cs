//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
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
            catch (NotFoundCourseAttachmentException notFoundCourseAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundCourseAttachmentException);
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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCourseAttachmentException =
                    new LockedCourseAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedCourseAttachmentException);
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

        private CourseAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var CourseAttachmentServiceException = new CourseAttachmentServiceException(exception);
            this.loggingBroker.LogError(CourseAttachmentServiceException);

            return CourseAttachmentServiceException;
        }

    }
}
