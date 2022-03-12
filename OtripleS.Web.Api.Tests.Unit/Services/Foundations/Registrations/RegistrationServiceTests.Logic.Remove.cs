// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async Task ShouldRemoveRegistrationByIdAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime: randomDateTime);
            Registration inputRegistration = randomRegistration;
            Guid inputRegistrationId = inputRegistration.Id;
            Registration storageRegistration = inputRegistration;
            Registration expectedRegistration = storageRegistration;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(inputRegistrationId))
                    .ReturnsAsync(expectedRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteRegistrationAsync(inputRegistration))
                    .ReturnsAsync(storageRegistration);

            // when
            Registration actualRegistration =
                await this.registrationService.
                    RemoveRegistrationByIdAsync(inputRegistrationId);

            // then
            actualRegistration.Should().BeEquivalentTo(expectedRegistration);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(inputRegistrationId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRegistrationAsync(inputRegistration),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
