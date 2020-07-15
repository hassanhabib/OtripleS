// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;

namespace OtripleS.Web.Api.Services.Courses
{
    public partial class CourseService
    {
        private delegate ValueTask<Course> ReturningCourseFunction();

        private async ValueTask<Course> TryCatch(ReturningCourseFunction returningCourseFunction)
        {
            try
            {
                return await returningCourseFunction();
            }
            catch (InvalidCourseInputException invalidCourseInputException)
            {
                throw CreateAndLogValidationException(invalidCourseInputException);
            }
            catch (NotFoundCourseException notFoundCourseException)
            {
                throw CreateAndLogValidationException(notFoundCourseException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCourseException = new LockedCourseException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedCourseException);
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

        private CourseValidationException CreateAndLogValidationException(Exception exception)
        {
            var courseValidationException = new CourseValidationException(exception);
            this.loggingBroker.LogError(courseValidationException);

            return courseValidationException;
        }

        private CourseDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var courseDependencyException = new CourseDependencyException(exception);
            this.loggingBroker.LogCritical(courseDependencyException);

            return courseDependencyException;
        }

        private CourseDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var courseDependencyException = new CourseDependencyException(exception);
            this.loggingBroker.LogError(courseDependencyException);

            return courseDependencyException;
        }

        private CourseServiceException CreateAndLogServiceException(Exception exception)
        {
            var courseServiceException = new CourseServiceException(exception);
            this.loggingBroker.LogError(courseServiceException);

            return courseServiceException;
        }
    }
}
