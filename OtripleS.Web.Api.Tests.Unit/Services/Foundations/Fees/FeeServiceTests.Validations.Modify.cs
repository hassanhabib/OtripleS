// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenFeeIsNullAndLogItAsync()
        {
            //given
            Fee invalidFee = null;
            var nullFeeException = new NullFeeException();

            var expectedFeeValidationException =
                new FeeValidationException(nullFeeException);

            //when
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(invalidFee);

            //then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                modifyFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenFeeIsInvalidAndLogItAsync(string invalidText)
        {
            //given
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

            invalidFeeException.UpsertDataList(
                key: nameof(Fee.UpdatedDate),
                value: $"Date is the same as {nameof(Fee.CreatedDate)}");

            invalidFeeException.UpsertDataList(
                key: nameof(Fee.UpdatedDate),
                value: "Date is required");

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(invalidFee.UpdatedDate);

            //when
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(invalidFee);

            //then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                modifyFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee invalidFee = randomFee;
            var invalidFeeException = new InvalidFeeException();

            invalidFeeException.AddData(
                key: nameof(Fee.UpdatedDate),
                values: $"Date is the same as {nameof(Fee.CreatedDate)}");

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(invalidFee.UpdatedDate);

            // when
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(invalidFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                modifyFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee inputFee = randomFee;
            inputFee.UpdatedBy = inputFee.CreatedBy;
            inputFee.UpdatedDate = dateTime.AddMinutes(minutes);
            var invalidFeeInputException = new InvalidFeeException();

            invalidFeeInputException.AddData(
                key: nameof(Fee.UpdatedDate),
                values: "Date is not recent");

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(inputFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                modifyFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfFeeDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee nonExistentFee = randomFee;
            nonExistentFee.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Fee noFee = null;
            var notFoundFeeException = new NotFoundFeeException(nonExistentFee.Id);

            var expectedFeeValidationException =
                new FeeValidationException(notFoundFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(nonExistentFee.Id))
                    .ReturnsAsync(noFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(nonExistentFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                modifyFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(nonExistentFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFeeAsync(It.IsAny<Fee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(randomDate);
            Fee invalidFee = randomFee;
            invalidFee.UpdatedDate = randomDate;
            Fee storageFee = randomFee.DeepClone();
            Guid feeId = invalidFee.Id;
            invalidFee.CreatedDate = storageFee.CreatedDate.AddMinutes(randomNumber);

            var invalidFeeException = new InvalidFeeException();

            invalidFeeException.AddData(
                key: nameof(Fee.CreatedDate),
                values: $"Date is not the same as {nameof(Fee.CreatedDate)}");

            var expectedFeeValidationException =
              new FeeValidationException(invalidFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(feeId))
                    .ReturnsAsync(storageFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(invalidFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                modifyFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(invalidFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDate = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(randomDate);
            randomFee.UpdatedDate = GetRandomDateTime();
            Fee invalidFee = randomFee;
            invalidFee.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            Fee storageFee = randomFee.DeepClone();
            Guid feeId = invalidFee.Id;
            invalidFee.CreatedBy = invalidCreatedBy;

            var invalidFeeException = new InvalidFeeException();

            invalidFeeException.AddData(
                key: nameof(Fee.CreatedBy),
                values: $"Id is not the same as {nameof(Fee.CreatedBy)}");

            var expectedFeeValidationException =
              new FeeValidationException(invalidFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(feeId))
                    .ReturnsAsync(storageFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(invalidFee.UpdatedDate);

            // when
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(invalidFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                modifyFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(invalidFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(randomDate);
            randomFee.UpdatedDate = GetRandomDateTime();
            randomFee.CreatedDate = randomFee.CreatedDate.AddMinutes(minutesInThePast);
            Fee invalidFee = randomFee;
            invalidFee.UpdatedDate = randomDate;
            Fee storageFee = randomFee.DeepClone();
            Guid feeId = invalidFee.Id;

            var invalidFeeException = new InvalidFeeException();

            invalidFeeException.AddData(
                key: nameof(Fee.UpdatedDate),
                values: $"Date is the same as {nameof(Fee.UpdatedDate)}");

            var expectedFeeValidationException =
              new FeeValidationException(invalidFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(feeId))
                    .ReturnsAsync(storageFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomFee.UpdatedDate);

            // when
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(invalidFee);

            // then
            await Assert.ThrowsAsync<FeeValidationException>(() =>
                modifyFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(invalidFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedFeeValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
