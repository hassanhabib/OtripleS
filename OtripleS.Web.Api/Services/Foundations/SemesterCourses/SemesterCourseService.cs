// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.SemesterCourses;

namespace OtripleS.Web.Api.Services.Foundations.SemesterCourses
{
    public partial class SemesterCourseService : ISemesterCourseService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public SemesterCourseService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }
        
        public ValueTask<SemesterCourse> CreateSemesterCourseAsync(SemesterCourse semesterCourse) =>
        TryCatch(async () =>
        {
            ValidateSemesterCourseOnCreate(semesterCourse);

            return await this.storageBroker.InsertSemesterCourseAsync(semesterCourse);
        });
        
        public IQueryable<SemesterCourse> RetrieveAllSemesterCourses() =>
        TryCatch(() => this.storageBroker.SelectAllSemesterCourses());

        public ValueTask<SemesterCourse> RetrieveSemesterCourseByIdAsync(Guid semesterCourseId) =>
        TryCatch(async () =>
        {
            ValidateSemesterCourseId(semesterCourseId);

            SemesterCourse maybeSemesterCourse =
                await this.storageBroker.SelectSemesterCourseByIdAsync(semesterCourseId);

            ValidateStorageSemesterCourse(maybeSemesterCourse, semesterCourseId);

            return maybeSemesterCourse;
        });
        
        public ValueTask<SemesterCourse> ModifySemesterCourseAsync(SemesterCourse semesterCourse) =>
        TryCatch(async () =>
        {
            ValidateSemesterCourseOnModify(semesterCourse);

            SemesterCourse maybeSemesterCourse =
                await this.storageBroker.SelectSemesterCourseByIdAsync(semesterCourse.Id);

            ValidateStorageSemesterCourse(maybeSemesterCourse, semesterCourse.Id);

            ValidateAgainstStorageSemesterCourseOnModify(
                inputSemesterCourse: semesterCourse,
                storageSemesterCourse: maybeSemesterCourse);

            return await this.storageBroker.UpdateSemesterCourseAsync(semesterCourse);
        });
        
        public ValueTask<SemesterCourse> RemoveSemesterCourseByIdAsync(Guid semesterCourseId) =>
        TryCatch(async () =>
        {
            ValidateSemesterCourseId(semesterCourseId);

            SemesterCourse maybeSemesterCourse =
                await this.storageBroker.SelectSemesterCourseByIdAsync(semesterCourseId);

            ValidateStorageSemesterCourse(maybeSemesterCourse, semesterCourseId);

            return await this.storageBroker.DeleteSemesterCourseAsync(maybeSemesterCourse);
        });
    }
}