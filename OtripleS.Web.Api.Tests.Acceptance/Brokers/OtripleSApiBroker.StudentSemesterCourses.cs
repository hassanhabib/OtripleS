// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentSemesterCourses;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string studentSemesterCourseRelativeUrl = "api/StudentSemesterCourses";

        public async ValueTask<StudentSemesterCourse> PostStudentSemesterCourseAsync(StudentSemesterCourse studentSemesterCourse) =>
            await this.apiFactoryClient.PostContentAsync(studentSemesterCourseRelativeUrl,studentSemesterCourse);

        public async ValueTask<StudentSemesterCourse> GetStudentSemesterCourseByIdAsync(Guid studentId, Guid semesterCourseId)=>
            await this.apiFactoryClient
                  .GetContentAsync<StudentSemesterCourse>($"{studentSemesterCourseRelativeUrl}/{studentId}/{semesterCourseId}");

        public async ValueTask<StudentSemesterCourse> DeleteStudentSemesterCourseAsync(Guid studentId, Guid semesterCourseId) =>
            await this.apiFactoryClient
            .DeleteContentAsync<StudentSemesterCourse>($"{studentSemesterCourseRelativeUrl}/{semesterCourseId}/{studentId}");

        public async ValueTask<StudentSemesterCourse> PutStudentSemesterCourseAsync(StudentSemesterCourse studentSemesterCourse) =>
            await this.apiFactoryClient.PutContentAsync(studentSemesterCourseRelativeUrl, studentSemesterCourse);

        public async ValueTask<List<StudentSemesterCourse>> GetAllStudentSemesterCourses() =>
            await this.apiFactoryClient.GetContentAsync<List<StudentSemesterCourse>>(studentSemesterCourseRelativeUrl);
    }
}
