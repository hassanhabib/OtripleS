// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Guardians;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Guardians
{
    public partial class GuardianServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenGuardiansWasEmptyAndLogIt()
        {
            // given
            IQueryable<Guardian> emptyStorageGuardians = new List<Guardian>().AsQueryable();
            IQueryable<Guardian> expectedGuardians = emptyStorageGuardians;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuardians())
                    .Returns(expectedGuardians);

            // when
            IQueryable<Guardian> actualGuardians =
                this.guardianService.RetrieveAllGuardians();

            // then
            actualGuardians.Should().BeEquivalentTo(emptyStorageGuardians);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No guardians found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuardians(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
