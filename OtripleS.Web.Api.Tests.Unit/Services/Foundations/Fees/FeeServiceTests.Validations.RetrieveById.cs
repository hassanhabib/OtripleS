// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomFeeId = default;
            Guid inputFeeId = randomFeeId;
            var invalidFeeException = new InvalidFeeException();

            invalidFeeException.AddData(
                key: nameof(Fee.Id),
                values: "Id is required");

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeException);

            //when
            ValueTask<Fee> retrieveFeeByIdTask =
                this.feeService.RetrieveFeeByIdAsync(inputFeeId);

            //then
            await Assert.ThrowsAsync<FeeValidationException>(() => retrieveFeeByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageFeeIsNullAndLogItAsync()
        {
            //given
            Guid randomFeeId = Guid.NewGuid();
            Guid someFeeId = randomFeeId;
            Fee invalidStorageFee = null;
            var notFoundFeeException = new NotFoundFeeException(someFeeId);

            var expectedFeeValidationException =
                new FeeValidationException(notFoundFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageFee);

            //when
            ValueTask<Fee> retrieveFeeByIdTask =
                this.feeService.RetrieveFeeByIdAsync(someFeeId);

            //then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                retrieveFeeByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}