// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Fees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenFeesWasEmptyAndLogIt()
        {
            // given
            IQueryable<Fee> emptyStorageFees = new List<Fee>().AsQueryable();
            IQueryable<Fee> expectedFees = emptyStorageFees;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFees())
                    .Returns(expectedFees);

            // when
            IQueryable<Fee> actualFees =
                this.feeService.RetrieveAllFees();

            // then
            actualFees.Should().BeEquivalentTo(emptyStorageFees);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No fees found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFees(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
