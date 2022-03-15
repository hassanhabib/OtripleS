// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Guardians;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Guardians
{
    public partial class GuardianServiceTests
    {
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
                await this.guardianService.RemoveGuardianByIdAsync(guardianId);

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
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
