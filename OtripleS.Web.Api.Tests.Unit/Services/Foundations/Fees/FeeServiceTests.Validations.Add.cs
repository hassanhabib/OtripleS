// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfFeeIsNullAndLogItAsync()
        {
            // given
            Fee randomFee = default;
            Fee nullFee = randomFee;
            var nullFeeException = new NullFeeException();

            var expectedFeeValidationException =
                new FeeValidationException(nullFeeException);

            // when
            ValueTask<Fee> addFeeTask =
                this.feeService.AddFeeAsync(nullFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                addFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnAddIfFeeIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidFee = new Fee
            {
                Label = invalidText
            };

            var invalidFeeException = new InvalidFeeException();

            invalidFeeException.AddData(
                key: nameof(Fee.Id),
                values: "Id is required");

            invalidFeeException.AddData(
                key: nameof(Fee.Label),
                values: "Text is required");

            invalidFeeException.AddData(
                key: nameof(Fee.CreatedBy),
                values: "Id is required");

            invalidFeeException.AddData(
                key: nameof(Fee.UpdatedBy),
                values: "Id is required");

            invalidFeeException.AddData(
                key: nameof(Fee.CreatedDate),
                values: "Date is required");

            invalidFeeException.AddData(
                key: nameof(Fee.UpdatedDate),
                values: "Date is required");

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeException);

            // when
            ValueTask<Fee> addFeeTask =
                this.feeService.AddFeeAsync(invalidFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                addFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee invalidFee = randomFee;
            invalidFee.UpdatedBy = randomFee.CreatedBy;
            invalidFee.UpdatedDate = GetRandomDateTime();
            var invalidFeeInputException = new InvalidFeeException();

            invalidFeeInputException.AddData(
                key: nameof(Fee.UpdatedDate),
                values: $"Date is not the same as {nameof(Fee.CreatedDate)}");

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeInputException);

            // when
            ValueTask<Fee> addFeeTask =
                this.feeService.AddFeeAsync(invalidFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                addFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync
            (int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee invalidFee = randomFee;
            invalidFee.UpdatedBy = invalidFee.CreatedBy;
            invalidFee.CreatedDate = dateTime.AddMinutes(minutes);
            invalidFee.UpdatedDate = invalidFee.CreatedDate;
            var invalidFeeException = new InvalidFeeException();

            invalidFeeException.AddData(
                key: nameof(Fee.CreatedDate),
                values: "Date is not recent");

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Fee> addFeeTask =
                this.feeService.AddFeeAsync(invalidFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                addFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfFeeAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee alreadyExistsFee = randomFee;
            alreadyExistsFee.UpdatedBy = alreadyExistsFee.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsFeeException =
                new AlreadyExistsFeeException(duplicateKeyException);

            var expectedFeeValidationException =
                new FeeValidationException(alreadyExistsFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFeeAsync(alreadyExistsFee))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Fee> addFeeTask =
                this.feeService.AddFeeAsync(alreadyExistsFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                addFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(alreadyExistsFee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
