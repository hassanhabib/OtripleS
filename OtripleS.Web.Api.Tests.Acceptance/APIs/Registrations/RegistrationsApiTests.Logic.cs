// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Registrations;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Registrations
{
    public partial class RegistrationsApiTests
    {
        [Fact]
        public async Task ShouldPostRegistrationAsync()
        {
            // given
            Registration randomRegistration = await CreateRandomRegistrationAsync();
            Registration inputRegistration = randomRegistration;
            Registration expectedRegistration = inputRegistration;

            // when 
            await this.otripleSApiBroker.PostRegistrationAsync(inputRegistration);

            Registration actualRegistration =
                await this.otripleSApiBroker.GetRegistrationByIdAsync(inputRegistration.Id);

            // then
            actualRegistration.Should().BeEquivalentTo(expectedRegistration);
            await this.otripleSApiBroker.DeleteRegistrationByIdAsync(actualRegistration.Id);
        }

        [Fact]
        public async Task ShouldPutRegistrationAsync()
        {
            // given
            Registration randomRegistration = await PostRandomRegistrationAsync();
            Registration modifiedRegistration = UpdateRegistrationRandom(randomRegistration);

            // when
            await this.otripleSApiBroker.PutRegistrationAsync(modifiedRegistration);

            Registration actualRegistration =
                await this.otripleSApiBroker.GetRegistrationByIdAsync(randomRegistration.Id);

            // then
            actualRegistration.Should().BeEquivalentTo(modifiedRegistration);
            await this.otripleSApiBroker.DeleteRegistrationByIdAsync(actualRegistration.Id);
        }

        [Fact]
        public async Task ShouldGetAllRegistrationsAsync()
        {
            // given
            var randomRegistrations = new List<Registration>();

            for (int i = 0; i <= GetRandomNumber(); i++)
            {
                randomRegistrations.Add(await PostRandomRegistrationAsync());
            }

            List<Registration> inputRegistrations = randomRegistrations;
            List<Registration> expectedRegistrations = inputRegistrations.ToList();

            // when
            List<Registration> actualRegistrations =
                await this.otripleSApiBroker.GetAllRegistrationsAsync();

            // then
            foreach (Registration expectedRegistration in expectedRegistrations)
            {
                Registration actualRegistration =
                    actualRegistrations.Single(registration =>
                        registration.Id == expectedRegistration.Id);

                actualRegistration.Should().BeEquivalentTo(expectedRegistration);
                await this.otripleSApiBroker.DeleteRegistrationByIdAsync(actualRegistration.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteRegistrationAsync()
        {
            // given
            Registration randomRegistration = await PostRandomRegistrationAsync();
            Registration inputRegistration = randomRegistration;
            Registration expectedRegistration = inputRegistration;

            // when 
            Registration deletedRegistration =
                await this.otripleSApiBroker.DeleteRegistrationByIdAsync(inputRegistration.Id);

            ValueTask<Registration> getRegistrationByIdTask =
                this.otripleSApiBroker.GetRegistrationByIdAsync(inputRegistration.Id);

            // then
            deletedRegistration.Should().BeEquivalentTo(expectedRegistration);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getRegistrationByIdTask.AsTask());
        }
    }
}
