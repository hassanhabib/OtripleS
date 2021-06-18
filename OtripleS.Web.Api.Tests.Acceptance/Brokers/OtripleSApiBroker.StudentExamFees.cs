// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.StudentExamFees;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string StudentExamFeesRelativeUrl = "api/studentexamfees";

        public async ValueTask<StudentExamFee> PostStudentExamFeeAsync(StudentExamFee studentExamFee) =>
            await this.apiFactoryClient.PostContentAsync(
                StudentExamFeesRelativeUrl,
                studentExamFee);

        public async ValueTask<StudentExamFee> GetStudentExamFeeByIdsAsync(
            Guid studentId,
            Guid examFeeId) =>
                await this.apiFactoryClient.GetContentAsync<StudentExamFee>(
                    $"{StudentExamFeesRelativeUrl}/studentid/{studentId}/examfeeid/{examFeeId}");

        public async ValueTask<StudentExamFee> DeleteStudentExamFeeByIdsAsync(
            Guid studentId,
            Guid examFeeId) =>
                await this.apiFactoryClient.DeleteContentAsync<StudentExamFee>(
                    $"{StudentExamFeesRelativeUrl}/studentid/{studentId}/examfeeid/{examFeeId}");

        public async ValueTask<StudentExamFee> PutStudentExamFeeAsync(StudentExamFee studentExamFee) =>
            await this.apiFactoryClient.PutContentAsync(
                StudentExamFeesRelativeUrl,
                studentExamFee);

        public async ValueTask<List<StudentExamFee>> GetAllStudentExamFeesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<StudentExamFee>>(
                $"{StudentExamFeesRelativeUrl}/");
    }
}
