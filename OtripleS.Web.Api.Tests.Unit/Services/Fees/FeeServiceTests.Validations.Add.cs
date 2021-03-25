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

namespace OtripleS.Web.Api.Tests.Unit.Services.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenFeeIsNullAndLogItAsync()
        {
            // given
            Fee randomFee = default;
            Fee nullFee = randomFee;
            var nullFeeException = new NullFeeException();

            var expectedFeeValidationException =
                new FeeValidationException(nullFeeException);

            // when
            ValueTask<Fee> createFeeTask =
                this.feeService.AddFeeAsync(nullFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                createFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenFeeIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee inputFee = randomFee;
            inputFee.Id = default;

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.Id),
                parameterValue: inputFee.Id);

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeInputException);

            // when
            ValueTask<Fee> createFeeTask =
                this.feeService.AddFeeAsync(inputFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                createFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee inputFee = randomFee;
            inputFee.CreatedBy = default;

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.CreatedBy),
                parameterValue: inputFee.CreatedBy);

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeInputException);

            // when
            ValueTask<Fee> createFeeTask =
                this.feeService.AddFeeAsync(inputFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                createFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                 broker.InsertFeeAsync(It.IsAny<Fee>()),
                     Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee inputFee = randomFee;
            inputFee.UpdatedBy = default;

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.UpdatedBy),
                parameterValue: inputFee.UpdatedBy);

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeInputException);

            // when
            ValueTask<Fee> createFeeTask =
                this.feeService.AddFeeAsync(inputFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                createFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee inputFee = randomFee;
            inputFee.CreatedDate = default;

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.CreatedDate),
                parameterValue: inputFee.CreatedDate);

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeInputException);

            // when
            ValueTask<Fee> createFeeTask =
                this.feeService.AddFeeAsync(inputFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                createFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee inputFee = randomFee;
            inputFee.UpdatedDate = default;

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.UpdatedDate),
                parameterValue: inputFee.UpdatedDate);

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeInputException);

            // when
            ValueTask<Fee> createFeeTask =
                this.feeService.AddFeeAsync(inputFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                createFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee inputFee = randomFee;
            inputFee.UpdatedBy = randomFee.CreatedBy;
            inputFee.UpdatedDate = GetRandomDateTime();

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.UpdatedDate),
                parameterValue: inputFee.UpdatedDate);

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeInputException);

            // when
            ValueTask<Fee> createFeeTask =
                this.feeService.AddFeeAsync(inputFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                createFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
