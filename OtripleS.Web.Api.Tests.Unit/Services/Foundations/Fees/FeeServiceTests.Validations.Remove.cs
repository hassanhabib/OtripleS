// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Foundations.Fees;
using OtripleS.Web.Api.Models.Foundations.Fees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomFeeId = default;
            Guid inputFeeId = randomFeeId;

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.Id),
                parameterValue: inputFeeId);

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeInputException);

            // when
            ValueTask<Fee> actualFeeTask =
                this.feeService.RemoveFeeByIdAsync(inputFeeId);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() => actualFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenStorageFeeIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Guid inputFeeId = randomFee.Id;
            Fee inputFee = randomFee;
            Fee nullStorageFee = null;

            var notFoundFeeException = new NotFoundFeeException(inputFeeId);

            var expectedFeeValidationException =
                new FeeValidationException(notFoundFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(inputFeeId))
                    .ReturnsAsync(nullStorageFee);

            // when
            ValueTask<Fee> actualFeeTask =
                this.feeService.RemoveFeeByIdAsync(inputFeeId);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() => actualFeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(inputFeeId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
