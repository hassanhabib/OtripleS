// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Courses;

namespace OtripleS.Web.Api.Services.Foundations.Courses
{
    public partial class CourseService : ICourseService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CourseService(IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Course> CreateCourseAsync(Course course) =>
        TryCatch(async () =>
        {
            ValidateCourseOnCreate(course);

            return await this.storageBroker.InsertCourseAsync(course);
        });

        public ValueTask<Course> ModifyCourseAsync(Course course) =>
        TryCatch(async () =>
        {
            ValidateCourseOnModify(course);
            Course maybeCourse = await this.storageBroker.SelectCourseByIdAsync(course.Id);
            ValidateStorageCourse(maybeCourse, course.Id);
            ValidateAgainstStorageCourseOnModify(inputCourse: course, storageCourse: maybeCourse);

            return await this.storageBroker.UpdateCourseAsync(course);
        });

        public ValueTask<Course> RemoveCourseAsync(Guid courseId) =>
        TryCatch(async () =>
        {
            ValidateCourseId(courseId);

            Course maybeCourse =
               await this.storageBroker.SelectCourseByIdAsync(courseId);

            ValidateStorageCourse(maybeCourse, courseId);

            return await this.storageBroker.DeleteCourseAsync(maybeCourse);
        });

        public IQueryable<Course> RetrieveAllCourses() =>
        TryCatch(() =>
        {
            IQueryable<Course> storageCourses = this.storageBroker.SelectAllCourses();
            ValidateStorageCourses(storageCourses);

            return storageCourses;
        });

        public ValueTask<Course> RetrieveCourseByIdAsync(Guid courseId) =>
        TryCatch(async () =>
        {
            ValidateCourseId(courseId);

            Course maybeCourse =
                await this.storageBroker.SelectCourseByIdAsync(courseId);

            ValidateStorageCourse(maybeCourse, courseId);

            return maybeCourse;
        });
    }
}
