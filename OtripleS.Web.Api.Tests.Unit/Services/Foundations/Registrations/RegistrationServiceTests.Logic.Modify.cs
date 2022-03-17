// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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
