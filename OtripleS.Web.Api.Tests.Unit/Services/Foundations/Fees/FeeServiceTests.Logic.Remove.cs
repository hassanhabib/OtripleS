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
        public async Task ShouldRemoveFeeAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Guid inputFeeId = randomFee.Id;
            Fee inputFee = randomFee;
            Fee storageFee = randomFee;
            Fee expectedFee = randomFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(inputFeeId))
                    .ReturnsAsync(inputFee);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteFeeAsync(inputFee))
                    .ReturnsAsync(storageFee);

            // when
            Fee actualFee =
                await this.feeService.RemoveFeeByIdAsync(inputFeeId);

            // then
            actualFee.Should().BeEquivalentTo(expectedFee);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(inputFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFeeAsync(inputFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
