// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Fees;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Fees
{
    public partial class FeesApiTests
    {
        [Fact]
        public async Task ShouldPostFeeAsync()
        {
            // given
            Fee randomFee = await CreateRandomFeeAsync();
            Fee inputFee = randomFee;
            Fee expectedFee = inputFee;

            // when 
            await this.otripleSApiBroker.PostFeeAsync(inputFee);

            Fee actualFee =
               await this.otripleSApiBroker.GetFeeByIdAsync(inputFee.Id);

            // then
            actualFee.Should().BeEquivalentTo(expectedFee);
            await DeleteFeeAsync(actualFee);
        }

        [Fact]
        public async Task ShouldPutFeeAsync()
        {
            // given
            Fee randomFee = await PostRandomFeeAsync();
            Fee modifiedFee = await UpdateFeeRandom(randomFee);

            // when
            await this.otripleSApiBroker.PutFeeAsync(modifiedFee);

            Fee actualFee =
                await this.otripleSApiBroker.GetFeeByIdAsync(randomFee.Id);

            // then
            actualFee.Should().BeEquivalentTo(modifiedFee);
            await DeleteFeeAsync(actualFee);
        }

        [Fact]
        public async Task ShouldGetAllFeesAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            var randomFees = new List<Fee>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomFees.Add(await PostRandomFeeAsync());
            }

            List<Fee> inputFees = randomFees;
            List<Fee> expectedFees = inputFees.ToList();

            // when
            List<Fee> actualFees = await this.otripleSApiBroker.GetAllFeesAsync();

            // then
            foreach (Fee expectedFee in expectedFees)
            {
                Fee actualFee = actualFees.Single(student => student.Id == expectedFee.Id);
                actualFee.Should().BeEquivalentTo(expectedFee);
                await DeleteFeeAsync(actualFee);
            }
        }

        [Fact]
        public async Task ShouldDeleteFeeAsync()
        {
            // given
            Fee randomFee = await PostRandomFeeAsync();
            Fee inputFee = randomFee;
            Fee expectedFee = inputFee;

            // when 
            Fee deletedFee = await DeleteFeeAsync(inputFee);

            ValueTask<Fee> getFeeByIdTask =
                this.otripleSApiBroker.GetFeeByIdAsync(inputFee.Id);

            // then
            deletedFee.Should().BeEquivalentTo(expectedFee);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getFeeByIdTask.AsTask());
        }
    }
}
