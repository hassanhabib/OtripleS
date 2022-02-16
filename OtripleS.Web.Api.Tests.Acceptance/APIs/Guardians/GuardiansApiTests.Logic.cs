// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Guardians;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Guardians
{
    public partial class GuardiansApiTests
    {
        [Fact]
        public async Task ShouldPostGuardianAsync()
        {
            // given
            Guardian randomGuardian = CreateRandomGuardian();
            Guardian inputGuardian = randomGuardian;
            Guardian expectedGuardian = inputGuardian;

            // when 
            await this.otripleSApiBroker.PostGuardianAsync(inputGuardian);

            Guardian actualGuardian =
                await this.otripleSApiBroker.GetGuardianByIdAsync(inputGuardian.Id);

            // then
            actualGuardian.Should().BeEquivalentTo(expectedGuardian);
            await this.otripleSApiBroker.DeleteGuardianByIdAsync(actualGuardian.Id);
        }

        [Fact]
        public async Task ShouldPutGuardianAsync()
        {
            // given
            Guardian randomGuardian = await PostRandomGuardianAsync();
            Guardian modifiedGuardian = UpdateGuardianRandom(randomGuardian);

            // when
            await this.otripleSApiBroker.PutGuardianAsync(modifiedGuardian);

            Guardian actualGuardian =
                await this.otripleSApiBroker.GetGuardianByIdAsync(randomGuardian.Id);

            // then
            actualGuardian.Should().BeEquivalentTo(modifiedGuardian);
            await this.otripleSApiBroker.DeleteGuardianByIdAsync(actualGuardian.Id);
        }

        [Fact]
        public async Task ShouldGetAllGuardiansAsync()
        {
            // given
            var randomGuardians = new List<Guardian>();

            for (int i = 0; i <= GetRandomNumber(); i++)
            {
                randomGuardians.Add(await PostRandomGuardianAsync());
            }

            List<Guardian> inputGuardians = randomGuardians;
            List<Guardian> expectedGuardians = inputGuardians.ToList();

            // when
            List<Guardian> actualGuardians =
                await this.otripleSApiBroker.GetAllGuardiansAsync();

            // then
            foreach (Guardian expectedGuardian in expectedGuardians)
            {
                Guardian actualGuardian =
                    actualGuardians.Single(guardian =>
                        guardian.Id == expectedGuardian.Id);

                actualGuardian.Should().BeEquivalentTo(expectedGuardian);
                await this.otripleSApiBroker.DeleteGuardianByIdAsync(actualGuardian.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteGuardianAsync()
        {
            // given
            Guardian randomGuardian = await PostRandomGuardianAsync();
            Guardian inputGuardian = randomGuardian;
            Guardian expectedGuardian = inputGuardian;

            // when 
            Guardian deletedGuardian =
                await this.otripleSApiBroker.DeleteGuardianByIdAsync(inputGuardian.Id);

            ValueTask<Guardian> getGuardianByIdTask =
                this.otripleSApiBroker.GetGuardianByIdAsync(inputGuardian.Id);

            // then
            deletedGuardian.Should().BeEquivalentTo(expectedGuardian);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getGuardianByIdTask.AsTask());
        }
    }
}
