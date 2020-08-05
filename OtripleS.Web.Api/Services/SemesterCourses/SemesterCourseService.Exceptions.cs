using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
    public partial class SemesterCourseService
    {
        private delegate ValueTask<SemesterCourse> ReturningSemesterCourseFunction();

        private async ValueTask<SemesterCourse> TryCatch(ReturningSemesterCourseFunction returningStudentFunction)
        {
            try
            {
                return await returningStudentFunction();
            }
            catch (InvalidSemesterCourseInputException invalidSemesterCourseInputException)
            {
                throw CreateAndLogValidationException(invalidSemesterCourseInputException);
            }
            catch (NotFoundSemesterCourseException notFoundSemesterCourseException)
            {
                throw CreateAndLogValidationException(notFoundSemesterCourseException);
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

        private SemesterCourseServiceException CreateAndLogServiceException(Exception exception)
        {
            var semesterCourseServiceException = new SemesterCourseServiceException(exception);
            this.loggingBroker.LogError(semesterCourseServiceException);

            return semesterCourseServiceException;
        }
    }
}