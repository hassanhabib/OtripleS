// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Guardians;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Guardians
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
        public void ShouldRetrieveAllGuardians()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Guardian> randomGuardians = CreateRandomGuardians(randomDateTime);
            IQueryable<Guardian> storageGuardians = randomGuardians;
            IQueryable<Guardian> expectedGuardians = storageGuardians;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuardians())
                    .Returns(storageGuardians);

            // when
            IQueryable<Guardian> actualGuardians =
                this.guardianService.RetrieveAllGuardians();

            // then
            actualGuardians.Should().BeEquivalentTo(expectedGuardians);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuardians(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
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

        [Fact]
        public async Task ShouldAddGuardianAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Guardian randomGuardian = CreateRandomGuardian(randomDateTime);
            randomGuardian.UpdatedBy = randomGuardian.CreatedBy;
            Guardian inputGuardian = randomGuardian;
            Guardian storageGuardian = randomGuardian;
            Guardian expectedGuardian = storageGuardian;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianAsync(inputGuardian))
                    .ReturnsAsync(storageGuardian);

            // when
            Guardian actualGuardian =
                await this.guardianService.CreateGuardianAsync(inputGuardian);

            // then
            actualGuardian.Should().BeEquivalentTo(expectedGuardian);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(inputGuardian),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteGuardianAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(randomDateTime);
            Guid guardianId = randomGuardian.Id;
            Guardian storageGuardian = randomGuardian;
            Guardian expectedGuardian = storageGuardian;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(guardianId))
                    .ReturnsAsync(storageGuardian);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteGuardianAsync(storageGuardian))
                    .ReturnsAsync(expectedGuardian);

            // when
            Guardian actualGuardian =
                await this.guardianService.DeleteGuardianByIdAsync(guardianId);

            // then
            actualGuardian.Should().BeEquivalentTo(expectedGuardian);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(guardianId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianAsync(storageGuardian),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
