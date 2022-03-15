// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Guardians;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Guardians
{
    public partial class GuardianServiceTests
    {
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
    }
}
