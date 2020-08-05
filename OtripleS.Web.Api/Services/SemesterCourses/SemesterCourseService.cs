// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
    public partial class SemesterCourseService : ISemesterCourseService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public SemesterCourseService(IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async ValueTask<SemesterCourse> DeleteSemesterCourseAsync(Guid semesterCourseId)
        {
            try
            {
                ValidateSemesterCourseServiceIdIsNull(semesterCourseId);
                SemesterCourse maybeSemesterCourse =
                    await this.storageBroker.SelectSemesterCourseByIdAsync(semesterCourseId);
                ValidateStorageSemesterCourse(maybeSemesterCourse, semesterCourseId);
                return await this.storageBroker.DeleteSemesterCourseAsync(maybeSemesterCourse);
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
        }
    }
}