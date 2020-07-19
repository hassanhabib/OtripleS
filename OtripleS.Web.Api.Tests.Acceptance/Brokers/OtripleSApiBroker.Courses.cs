// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string CoursesRelativeUrl = "api/courses";

        public async ValueTask<Course> PostCourseAsync(Course course) =>
            await this.apiFactoryClient.PostContentAsync(CoursesRelativeUrl, course);

        public async ValueTask<Course> GetCourseByIdAsync(Guid CourseId) =>
            await this.apiFactoryClient.GetContentAsync<Course>($"{CoursesRelativeUrl}/{CourseId}");

        public async ValueTask<Course> DeleteCourseByIdAsync(Guid CourseId) =>
            await this.apiFactoryClient.DeleteContentAsync<Course>($"{CoursesRelativeUrl}/{CourseId}");

        public async ValueTask<Course> PutCourseAsync(Course course) =>
            await this.apiFactoryClient.PutContentAsync(CoursesRelativeUrl, course);

        public async ValueTask<List<Course>> GetAllCoursesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Course>>($"{CoursesRelativeUrl}/");

    }
}
