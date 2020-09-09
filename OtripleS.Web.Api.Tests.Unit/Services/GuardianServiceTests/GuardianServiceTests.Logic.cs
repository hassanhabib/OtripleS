// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(inputGuardian),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
