// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async Task ShouldAddRegistrationAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Registration randomRegistration = CreateRandomRegistration(randomDateTime);
            randomRegistration.UpdatedBy = randomRegistration.CreatedBy;
            Registration inputRegistration = randomRegistration;
            Registration storageRegistration = randomRegistration;
            Registration expectedRegistration = storageRegistration;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRegistrationAsync(inputRegistration))
                    .ReturnsAsync(storageRegistration);

            // when
            Registration actualRegistration =
                await this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            actualRegistration.Should().BeEquivalentTo(expectedRegistration);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(inputRegistration),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
