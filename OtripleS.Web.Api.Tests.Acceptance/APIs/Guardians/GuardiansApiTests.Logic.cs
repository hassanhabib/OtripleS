using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.Guardians;
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
    }
}
