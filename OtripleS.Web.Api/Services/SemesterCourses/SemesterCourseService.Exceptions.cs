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
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
    public partial class SemesterCourseService
    {
        private delegate ValueTask<SemesterCourse> ReturningSemesterCourseFunction();
        private delegate IQueryable<SemesterCourse> ReturningSemesterCoursesFunction();

        private async ValueTask<SemesterCourse> TryCatch(
            ReturningSemesterCourseFunction returningSemesterCourseFunction)
        {
            try
            {
                return await returningSemesterCourseFunction();
            }
            catch (NullSemesterCourseException nullSemesterCourseException)
            {
                throw CreateAndLogValidationException(nullSemesterCourseException);
            }
            catch (InvalidSemesterCourseInputException invalidSemesterCourseInputException)
            {
                throw CreateAndLogValidationException(invalidSemesterCourseInputException);
            }
            catch (NotFoundSemesterCourseException nullSemesterCourseException)
            {
                throw CreateAndLogValidationException(nullSemesterCourseException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSemesterCourseException =
                    new AlreadyExistsSemesterCourseException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsSemesterCourseException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSemesterCourseException = new LockedSemesterCourseException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedSemesterCourseException);
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

        private IQueryable<SemesterCourse> TryCatch(ReturningSemesterCoursesFunction returningSemesterCoursesFunction)
        {
            try
            {
                return returningSemesterCoursesFunction();
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

        private SemesterCourseServiceException CreateAndLogServiceException(Exception exception)
        {
            var semesterCourseServiceException = new SemesterCourseServiceException(exception);
            this.loggingBroker.LogError(semesterCourseServiceException);

            return semesterCourseServiceException;
        }

        private SemesterCourseValidationException CreateAndLogValidationException(Exception exception)
        {
            var semesterCourseValidationException = new SemesterCourseValidationException(exception);
            this.loggingBroker.LogError(semesterCourseValidationException);

            return semesterCourseValidationException;
        }

        private SemesterCourseDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var semesterCourseDependencyException = new SemesterCourseDependencyException(exception);
            this.loggingBroker.LogCritical(semesterCourseDependencyException);

            return semesterCourseDependencyException;
        }

        private SemesterCourseDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var semesterCourseDependencyException = new SemesterCourseDependencyException(exception);
            this.loggingBroker.LogError(semesterCourseDependencyException);

            return semesterCourseDependencyException;
        }
    }
}