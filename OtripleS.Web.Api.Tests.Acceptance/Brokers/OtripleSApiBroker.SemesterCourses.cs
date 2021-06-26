// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.SemesterCourses;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string SemesterCoursesRelativeUrl = "api/semesterCourses";

        public async ValueTask<SemesterCourse> PostSemesterCourseAsync(SemesterCourse semesterCourse) =>
            await this.apiFactoryClient.PostContentAsync(SemesterCoursesRelativeUrl, semesterCourse);

        public async ValueTask<SemesterCourse> GetSemesterCourseByIdAsync(Guid semesterCourseId) =>
            await this.apiFactoryClient.GetContentAsync<SemesterCourse>($"{SemesterCoursesRelativeUrl}/{semesterCourseId}");

        public async ValueTask<SemesterCourse> DeleteSemesterCourseByIdAsync(Guid semesterCourseId) =>
            await this.apiFactoryClient.DeleteContentAsync<SemesterCourse>($"{SemesterCoursesRelativeUrl}/{semesterCourseId}");

        public async ValueTask<SemesterCourse> PutSemesterCourseAsync(SemesterCourse semesterCourse) =>
            await this.apiFactoryClient.PutContentAsync(SemesterCoursesRelativeUrl, semesterCourse);

        public async ValueTask<List<SemesterCourse>> GetAllSemesterCoursesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<SemesterCourse>>($"{SemesterCoursesRelativeUrl}/");
    }
}
