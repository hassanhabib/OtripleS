// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Registrations
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

        [Fact]
        public void ShouldRetrieveAllRegistrations()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Registration> randomRegistrations =
                CreateRandomRegistrations(randomDateTime);

            IQueryable<Registration> storageRegistrations =
                randomRegistrations;

            IQueryable<Registration> expectedRegistrations =
                storageRegistrations;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRegistrations())
                    .Returns(storageRegistrations);

            // when
            IQueryable<Registration> actualRegistrations =
                this.registrationService.RetrieveAllRegistrations();

            // then
            actualRegistrations.Should().BeEquivalentTo(expectedRegistrations);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRegistrations(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveRegistrationByIdAsync()
        {
            // given
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(randomDateTime);
            Registration storageRegistration = randomRegistration;
            Registration expectedRegistration = storageRegistration;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(inputRegistrationId))
                    .ReturnsAsync(storageRegistration);

            // when
            Registration actualRegistration =
                await this.registrationService.RetrieveRegistrationByIdAsync(inputRegistrationId);

            // then
            actualRegistration.Should().BeEquivalentTo(expectedRegistration);

            this.storageBrokerMock.Verify(broker =>
                 broker.SelectRegistrationByIdAsync(inputRegistrationId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyRegistrationAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(randomInputDate);
            Registration inputRegistration = randomRegistration;
            Registration afterUpdateStorageRegistration = inputRegistration;
            Registration expectedRegistration = afterUpdateStorageRegistration;
            Registration beforeUpdateStorageRegistration = randomRegistration.DeepClone();
            inputRegistration.UpdatedDate = randomDate;
            Guid guardianId = inputRegistration.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(guardianId))
                    .ReturnsAsync(beforeUpdateStorageRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateRegistrationAsync(inputRegistration))
                    .ReturnsAsync(afterUpdateStorageRegistration);

            // when
            Registration actualRegistration =
                await this.registrationService.ModifyRegistrationAsync(inputRegistration);

            // then
            actualRegistration.Should().BeEquivalentTo(expectedRegistration);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(guardianId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(inputRegistration),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
