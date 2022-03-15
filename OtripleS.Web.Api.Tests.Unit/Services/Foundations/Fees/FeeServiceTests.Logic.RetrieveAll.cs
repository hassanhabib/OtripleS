// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Fees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllFees()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Fee> randomFees =
                CreateRandomFees(randomDateTime);

            IQueryable<Fee> storageFees =
                randomFees;

            IQueryable<Fee> expectedFees =
                storageFees;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFees())
                    .Returns(storageFees);

            // when
            IQueryable<Fee> actualFees =
                this.feeService.RetrieveAllFees();

            // then
            actualFees.Should().BeEquivalentTo(expectedFees);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFees(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
