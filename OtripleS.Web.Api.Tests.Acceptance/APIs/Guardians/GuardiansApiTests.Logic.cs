// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Guardians;
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
            Guardian randomGuardian = CreateRandomGuardian();
            await this.otripleSApiBroker.PostGuardianAsync(randomGuardian);
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
            IEnumerable<Guardian> randomGuardians = GetRandomGuardians();
            IEnumerable<Guardian> inputGuardians = randomGuardians;

            foreach (Guardian guardian in inputGuardians)
            {
                await this.otripleSApiBroker.PostGuardianAsync(guardian);
            }

            List<Guardian> expectedGuardians = inputGuardians.ToList();

            // when
            List<Guardian> actualGuardians = await this.otripleSApiBroker.GetAllGuardiansAsync();

            // then
            foreach (Guardian expectedGuardian in expectedGuardians)
            {
                Guardian actualGuardian = actualGuardians.Single(guardian => guardian.Id == expectedGuardian.Id);
                actualGuardian.Should().BeEquivalentTo(expectedGuardian);
                await this.otripleSApiBroker.DeleteGuardianByIdAsync(actualGuardian.Id);
            }
        }
    }
}
