// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenExamFeeIsNullAndLogItAsync()
        {
            //given
            ExamFee invalidExamFee = null;
            var nullExamFeeException = new NullExamFeeException();

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(nullExamFeeException);

            //when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(invalidExamFee);

            //then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                modifyExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenIdsAreInvalidAndLogItAsync()
        {
            //given
            Guid invalidExamFeeId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee invalidExamFee = randomExamFee;
            invalidExamFee.Id = invalidExamFeeId;
            invalidExamFee.ExamId = invalidExamFeeId;
            invalidExamFee.FeeId = invalidExamFeeId;
            invalidExamFee.CreatedBy = default;
            invalidExamFee.UpdatedBy = default;

            var invalidExamFeeException = new InvalidExamFeeException();

            invalidExamFeeException.AddData(
                key: nameof(ExamFee.Id),
                values: "Id is required");

            invalidExamFeeException.AddData(
                key: nameof(ExamFee.ExamId),
                values: "Id is required");

            invalidExamFeeException.AddData(
                key: nameof(ExamFee.FeeId),
                values: "Id is required");

            invalidExamFeeException.AddData(
                key: nameof(ExamFee.CreatedBy),
                values: "Id is required");

            invalidExamFeeException.AddData(
                key: nameof(ExamFee.UpdatedBy),
                values: "Id is required");

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeException);

            //when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(invalidExamFee);

            //then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                modifyExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenDatesAreInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.CreatedDate = default;
            inputExamFee.UpdatedDate = default;

            var invalidExamFeeInputException = new InvalidExamFeeException();

            invalidExamFeeInputException.AddData(
                key: nameof(ExamFee.CreatedDate),
                values: "Date is required");

            invalidExamFeeInputException.AddData(
                key: nameof(ExamFee.UpdatedDate),
                values: "Date is required");

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                modifyExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

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
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.UpdatedBy = inputExamFee.CreatedBy;
            inputExamFee.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidExamFeeInputException = new InvalidExamFeeException();

            invalidExamFeeInputException.AddData(
                key: nameof(ExamFee.CreatedDate),
                values: $"Date is not recent");

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                modifyExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            var invalidExamFeeInputException = new InvalidExamFeeException();

            invalidExamFeeInputException.AddData(
                key: nameof(ExamFee.UpdatedDate),
                values: $"Date is the same as {nameof(ExamFee.CreatedDate)}");

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                modifyExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfExamFeeDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee nonExistentExamFee = randomExamFee;
            nonExistentExamFee.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            ExamFee noExamFee = null;
            var notFoundExamFeeException = new NotFoundExamFeeException(nonExistentExamFee.Id);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(notFoundExamFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(nonExistentExamFee.Id))
                    .ReturnsAsync(noExamFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(nonExistentExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                modifyExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(nonExistentExamFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamFeeAsync(It.IsAny<ExamFee>()),
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
            ExamFee randomExamFee = CreateRandomExamFee(randomDate);
            ExamFee invalidExamFee = randomExamFee;
            invalidExamFee.UpdatedDate = randomDate;
            ExamFee storageExamFee = randomExamFee.DeepClone();
            Guid examFeeId = invalidExamFee.Id;
            invalidExamFee.CreatedDate = storageExamFee.CreatedDate.AddMinutes(randomNumber);

            var invalidExamFeeException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.CreatedDate),
                parameterValue: invalidExamFee.CreatedDate);

            var expectedExamFeeValidationException =
              new ExamFeeValidationException(invalidExamFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(examFeeId))
                    .ReturnsAsync(storageExamFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(invalidExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                modifyExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(invalidExamFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
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
            ExamFee randomExamFee = CreateRandomExamFee(randomDate);
            ExamFee invalidExamFee = randomExamFee;
            invalidExamFee.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            ExamFee storageExamFee = randomExamFee.DeepClone();
            Guid examFeeId = invalidExamFee.Id;
            invalidExamFee.CreatedBy = invalidCreatedBy;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.CreatedBy),
                parameterValue: invalidExamFee.CreatedBy);

            var expectedExamFeeValidationException =
              new ExamFeeValidationException(invalidExamFeeInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(examFeeId))
                    .ReturnsAsync(storageExamFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(invalidExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                modifyExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(invalidExamFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
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
            ExamFee randomExamFee = CreateRandomExamFee(randomDate);
            randomExamFee.CreatedDate = randomExamFee.CreatedDate.AddMinutes(minutesInThePast);
            ExamFee invalidExamFee = randomExamFee;
            invalidExamFee.UpdatedDate = randomDate;
            ExamFee storageExamFee = randomExamFee.DeepClone();
            Guid examFeeId = invalidExamFee.Id;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.UpdatedDate),
                parameterValue: invalidExamFee.UpdatedDate);

            var expectedExamFeeValidationException =
              new ExamFeeValidationException(invalidExamFeeInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(examFeeId))
                    .ReturnsAsync(storageExamFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(invalidExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                modifyExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(invalidExamFee.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
