// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Guardian;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianServiceTests
{
    public partial class GuardianServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveGuardianByIdAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guid inputGuardianId = randomGuardian.Id;
            Guardian inputGuardian = randomGuardian;
            Guardian storageGuardian = inputGuardian;
            Guardian expectedGuardian = storageGuardian;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId))
                    .ReturnsAsync(storageGuardian);

            // when
            Guardian actualGuardian =
                await this.guardianService.RetrieveGuardianByIdAsync(inputGuardianId);

            // then
            actualGuardian.Should().BeEquivalentTo(expectedGuardian);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyGuardianAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(randomInputDate);
            Guardian inputGuardian = randomGuardian;
            Guardian afterUpdateStorageGuardian = inputGuardian;
            Guardian expectedGuardian = afterUpdateStorageGuardian;
            Guardian beforeUpdateStorageGuardian = randomGuardian.DeepClone();
            inputGuardian.UpdatedDate = randomDate;
            Guid guardianId = inputGuardian.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(guardianId))
                    .ReturnsAsync(beforeUpdateStorageGuardian);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuardianAsync(inputGuardian))
                    .ReturnsAsync(afterUpdateStorageGuardian);

            // when
            Guardian actualGuardian =
                await this.guardianService.ModifyGuardianAsync(inputGuardian);

            // then
            actualGuardian.Should().BeEquivalentTo(expectedGuardian);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(guardianId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuardianAsync(inputGuardian),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
