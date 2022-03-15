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
    }
}
