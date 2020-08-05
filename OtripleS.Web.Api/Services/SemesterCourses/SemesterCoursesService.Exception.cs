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
    }
}
