// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Courses;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string CoursesRelativeUrl = "api/courses";

        public async ValueTask<Course> PostCourseAsync(Course course) =>
            await this.apiFactoryClient.PostContentAsync(CoursesRelativeUrl, course);

        public async ValueTask<Course> GetCourseByIdAsync(Guid courseId) =>
            await this.apiFactoryClient.GetContentAsync<Course>($"{CoursesRelativeUrl}/{courseId}");

        public async ValueTask<Course> DeleteCourseByIdAsync(Guid courseId) =>
            await this.apiFactoryClient.DeleteContentAsync<Course>($"{CoursesRelativeUrl}/{courseId}");

        public async ValueTask<Course> PutCourseAsync(Course course) =>
            await this.apiFactoryClient.PutContentAsync(CoursesRelativeUrl, course);

        public async ValueTask<List<Course>> GetAllCoursesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Course>>($"{CoursesRelativeUrl}/");
    }
}
