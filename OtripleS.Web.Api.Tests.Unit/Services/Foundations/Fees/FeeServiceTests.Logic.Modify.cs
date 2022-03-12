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
        public async Task ShouldModifyFeeAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(randomInputDate);
            Fee inputFee = randomFee;
            Fee afterUpdateStorageFee = inputFee;
            Fee expectedFee = afterUpdateStorageFee;
            Fee beforeUpdateStorageFee = randomFee.DeepClone();
            inputFee.UpdatedDate = randomDate;
            Guid feeId = inputFee.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(feeId))
                    .ReturnsAsync(beforeUpdateStorageFee);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateFeeAsync(inputFee))
                    .ReturnsAsync(afterUpdateStorageFee);

            // when
            Fee actualFee =
                await this.feeService.ModifyFeeAsync(inputFee);

            // then
            actualFee.Should().BeEquivalentTo(expectedFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(feeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFeeAsync(inputFee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
