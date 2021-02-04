// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
            catch (InvalidCourseAttachmentException invalidCourseAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidCourseAttachmentInputException);
            }
            catch (NotFoundCourseAttachmentException notFoundCourseAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundCourseAttachmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
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
    }
}
