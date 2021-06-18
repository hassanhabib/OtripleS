// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.ExamFees;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.ExamFees
{
    public partial class ExamFeesApiTests
    {
        [Fact]
        public async Task ShouldPostExamFeeAsync()
        {
            // given
            ExamFee randomExamFee = await CreateRandomExamFeeAsync();
            ExamFee inputExamFee = randomExamFee;
            ExamFee expectedExamFee = inputExamFee;

            // when 
            await this.otripleSApiBroker.PostExamFeeAsync(inputExamFee);

            ExamFee actualExamFee =
               await this.otripleSApiBroker.GetExamFeeByIdAsync(inputExamFee.Id);

            // then
            actualExamFee.Should().BeEquivalentTo(expectedExamFee);
            await DeleteExamFeeAsync(actualExamFee);
        }

        [Fact]
        public async Task ShouldPutExamFeeAsync()
        {
            // given
            ExamFee randomExamFee = await PostRandomExamFeeAsync();
            ExamFee modifiedExamFee = await UpdateExamFeeRandom(randomExamFee);

            // when
            await this.otripleSApiBroker.PutExamFeeAsync(modifiedExamFee);

            ExamFee actualExamFee =
                await this.otripleSApiBroker.GetExamFeeByIdAsync(randomExamFee.Id);

            // then
            actualExamFee.Should().BeEquivalentTo(modifiedExamFee);
            await DeleteExamFeeAsync(actualExamFee);
        }

        [Fact]
        public async Task ShouldGetAllExamFeesAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            var randomExamFees = new List<ExamFee>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomExamFees.Add(await PostRandomExamFeeAsync());
            }

            List<ExamFee> inputExamFees = randomExamFees;
            List<ExamFee> expectedExamFees = inputExamFees.ToList();

            // when
            List<ExamFee> actualExamFees = await this.otripleSApiBroker.GetAllExamFeesAsync();

            // then
            foreach (ExamFee expectedExamFee in expectedExamFees)
            {
                ExamFee actualExamFee = actualExamFees.Single(student => student.Id == expectedExamFee.Id);
                actualExamFee.Should().BeEquivalentTo(expectedExamFee);
                await DeleteExamFeeAsync(actualExamFee);
            }
        }

        [Fact]
        public async Task ShouldDeleteExamFeeAsync()
        {
            // given
            ExamFee randomExamFee = await PostRandomExamFeeAsync();
            ExamFee inputExamFee = randomExamFee;
            ExamFee expectedExamFee = inputExamFee;

            // when 
            ExamFee deletedExamFee = await DeleteExamFeeAsync(inputExamFee);

            ValueTask<ExamFee> getExamFeeByIdTask =
                this.otripleSApiBroker.GetExamFeeByIdAsync(inputExamFee.Id);

            // then
            deletedExamFee.Should().BeEquivalentTo(expectedExamFee);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getExamFeeByIdTask.AsTask());
        }
    }
}
