// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Exams;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string ExamsRelativeUrl = "api/exams";

        public async ValueTask<Exam> PostExamAsync(Exam exam) =>
            await this.apiFactoryClient.PostContentAsync(ExamsRelativeUrl, exam);

        public async ValueTask<Exam> GetExamByIdAsync(Guid examId) =>
            await this.apiFactoryClient.GetContentAsync<Exam>($"{ExamsRelativeUrl}/{examId}");

        public async ValueTask<Exam> DeleteExamByIdAsync(Guid examId) =>
            await this.apiFactoryClient.DeleteContentAsync<Exam>($"{ExamsRelativeUrl}/{examId}");

        public async ValueTask<Exam> PutExamAsync(Exam exam) =>
            await this.apiFactoryClient.PutContentAsync(ExamsRelativeUrl, exam);

        public async ValueTask<List<Exam>> GetAllExamsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Exam>>($"{ExamsRelativeUrl}/");
    }
}
