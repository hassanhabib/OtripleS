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

namespace OtripleS.Web.Api.Tests.Unit.Services.Fees
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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenFeeIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidFeeId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee invalidFee = randomFee;
            invalidFee.Id = invalidFeeId;

            var invalidFeeException = new InvalidFeeException(
                parameterName: nameof(Fee.Id),
                parameterValue: invalidFee.Id);

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeException);

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
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenFeeLabelIsInvalidAndLogItAsync(
            string invalidFeeLabel)
        {
            // given
            Fee randomFee = CreateRandomFee(DateTime.Now);
            Fee invalidFee = randomFee;
            invalidFee.Label = invalidFeeLabel;

            var invalidFeeException = new InvalidFeeException(
               parameterName: nameof(Fee.Label),
               parameterValue: invalidFee.Label);

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeException);

            // when
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(invalidFee);

            // then
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

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
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
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(inputFee);

            // then
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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
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
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(inputFee);

            // then
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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(inputFee);

            // then
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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(inputFee);

            // then
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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(dateTime);
            Fee inputFee = randomFee;

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.UpdatedDate),
                parameterValue: inputFee.UpdatedDate);

            var expectedFeeValidationException =
                new FeeValidationException(invalidFeeInputException);

            // when
            ValueTask<Fee> modifyFeeTask =
                this.feeService.ModifyFeeAsync(inputFee);

            // then
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

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.UpdatedDate),
                parameterValue: inputFee.UpdatedDate);

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
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
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

            var invalidFeeException = new InvalidFeeException(
                parameterName: nameof(Fee.CreatedDate),
                parameterValue: invalidFee.CreatedDate);

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
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
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
            Fee invalidFee = randomFee;
            invalidFee.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            Fee storageFee = randomFee.DeepClone();
            Guid feeId = invalidFee.Id;
            invalidFee.CreatedBy = invalidCreatedBy;

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.CreatedBy),
                parameterValue: invalidFee.CreatedBy);

            var expectedFeeValidationException =
              new FeeValidationException(invalidFeeInputException);

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
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
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
            randomFee.CreatedDate = randomFee.CreatedDate.AddMinutes(minutesInThePast);
            Fee invalidFee = randomFee;
            invalidFee.UpdatedDate = randomDate;
            Fee storageFee = randomFee.DeepClone();
            Guid feeId = invalidFee.Id;

            var invalidFeeInputException = new InvalidFeeException(
                parameterName: nameof(Fee.UpdatedDate),
                parameterValue: invalidFee.UpdatedDate);

            var expectedFeeValidationException =
              new FeeValidationException(invalidFeeInputException);

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
                broker.LogError(It.Is(SameExceptionAs(expectedFeeValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
