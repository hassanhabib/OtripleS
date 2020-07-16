// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;

using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Courses;

namespace OtripleS.Web.Api.Services.Courses
{
    public partial class CourseService : ICourseService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public CourseService(IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Course> CreateCourseAsync(Course course) =>
        TryCatch(async () =>
        {
            ValidateCourseOnCreate(course);

            return await this.storageBroker.InsertCourseAsync(course);
        });

        public ValueTask<Course> DeleteCourseAsync(Guid courseId) =>
        TryCatch(async () =>
        {
            ValidateCourseId(courseId);

            Course maybeCourse =
               await this.storageBroker.SelectCourseByIdAsync(courseId);

            ValidateStorageCourse(maybeCourse, courseId);

            return await this.storageBroker.DeleteCourseAsync(maybeCourse);
        });
    }
}
