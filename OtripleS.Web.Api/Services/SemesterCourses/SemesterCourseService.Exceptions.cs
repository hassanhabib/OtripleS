using System;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
    public partial class SemesterCourseService
    {
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