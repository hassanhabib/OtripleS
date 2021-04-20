// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.ExamFees;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string ExamFeesRelativeUrl = "api/examfees";

        public async ValueTask<ExamFee> PostExamFeeAsync(ExamFee examFee) =>
            await this.apiFactoryClient.PostContentAsync(ExamFeesRelativeUrl, examFee);

        public async ValueTask<ExamFee> GetExamFeeByIdAsync(Guid examFeeId) =>
            await this.apiFactoryClient.GetContentAsync<ExamFee>($"{ExamFeesRelativeUrl}/{examFeeId}");

        public async ValueTask<ExamFee> DeleteExamFeeByIdAsync(Guid examFeeId) =>
            await this.apiFactoryClient.DeleteContentAsync<ExamFee>($"{ExamFeesRelativeUrl}/{examFeeId}");

        public async ValueTask<ExamFee> PutExamFeeAsync(ExamFee examFee) =>
            await this.apiFactoryClient.PutContentAsync(ExamFeesRelativeUrl, examFee);

        public async ValueTask<List<ExamFee>> GetAllExamFeesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<ExamFee>>($"{ExamFeesRelativeUrl}/");
    }
}
