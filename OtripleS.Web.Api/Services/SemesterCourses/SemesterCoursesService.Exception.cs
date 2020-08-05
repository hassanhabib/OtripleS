// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
	public partial class SemesterCourseService
	{
        private delegate ValueTask<SemesterCourse> ReturningSemesterCourseFunction();
        private async ValueTask<SemesterCourse> TryCatch(ReturningSemesterCourseFunction returningSemesterCourseFunction)
        {
            try
            {
                return await returningSemesterCourseFunction();
            }
            catch (InvalidSemesterCourseException invalidSemesterCourseInputException)
            {
                throw CreateAndLogValidationException(invalidSemesterCourseInputException);
            }
            catch (NotFoundSemesterCourseException nullSemesterCourseException)
            {
                throw CreateAndLogValidationException(nullSemesterCourseException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
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
    }
}
