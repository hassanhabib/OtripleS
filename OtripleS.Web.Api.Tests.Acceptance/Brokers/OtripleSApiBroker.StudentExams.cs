// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentExams;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string StudentExamsRelativeUrl = "api/studentexams";

        public async ValueTask<StudentExam> PostStudentExamAsync(StudentExam studentExam) =>
            await this.apiFactoryClient.PostContentAsync(StudentExamsRelativeUrl, studentExam);

        public async ValueTask<StudentExam> GetStudentExamByIdAsync(Guid studentExamId) =>
            await this.apiFactoryClient.GetContentAsync<StudentExam>($"{StudentExamsRelativeUrl}/{studentExamId}");

        public async ValueTask<StudentExam> DeleteStudentExamByIdAsync(Guid studentExamId) =>
            await this.apiFactoryClient.DeleteContentAsync<StudentExam>($"{StudentExamsRelativeUrl}/{studentExamId}");

        public async ValueTask<StudentExam> PutStudentExamAsync(StudentExam student) =>
            await this.apiFactoryClient.PutContentAsync(StudentExamsRelativeUrl, student);

        public async ValueTask<List<StudentExam>> GetAllStudentExamsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<StudentExam>>($"{StudentExamsRelativeUrl}/");
    }
}
