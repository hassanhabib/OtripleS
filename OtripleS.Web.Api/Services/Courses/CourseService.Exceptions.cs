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
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;

namespace OtripleS.Web.Api.Services.Courses
{
    public partial class CourseService
    {
        private delegate ValueTask<Course> ReturningCourseFunction();
        private delegate IQueryable<Course> ReturningQueryableCourseFunction();

        private async ValueTask<Course> TryCatch(ReturningCourseFunction returningCourseFunction)
        {
            try
            {
                return await returningCourseFunction();
            }
            catch (NullCourseException nullCourseException)
            {
                throw CreateAndLogValidationException(nullCourseException);
            }
            catch (InvalidCourseInputException invalidCourseInputException)
            {
                throw CreateAndLogValidationException(invalidCourseInputException);
            }
            catch (NotFoundCourseException nullCourseException)
            {
                throw CreateAndLogValidationException(nullCourseException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCourseException =
                    new AlreadyExistsCourseException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCourseException);
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

        private IQueryable<Course> TryCatch(ReturningQueryableCourseFunction returningQueryableCourseFunction)
        {
            try
            {
                return returningQueryableCourseFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
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

        private CourseServiceException CreateAndLogServiceException(Exception exception)
        {
            var courseServiceException = new CourseServiceException(exception);
            this.loggingBroker.LogError(courseServiceException);

            return courseServiceException;
        }

        private CourseDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var courseDependencyException = new CourseDependencyException(exception);
            this.loggingBroker.LogError(courseDependencyException);

            return courseDependencyException;
        }

        private CourseDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var courseDependencyException = new CourseDependencyException(exception);
            this.loggingBroker.LogCritical(courseDependencyException);

            return courseDependencyException;
        }

        private CourseValidationException CreateAndLogValidationException(Exception exception)
        {
            var courseValidationException = new CourseValidationException(exception);
            this.loggingBroker.LogError(courseValidationException);

            return courseValidationException;
        }
    }
}
