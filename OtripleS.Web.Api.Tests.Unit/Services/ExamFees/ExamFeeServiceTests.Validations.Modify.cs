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

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamFees
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
        public async Task ShouldThrowValidationExceptionOnModifyWhenExamFeeIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidExamFeeId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee invalidExamFee = randomExamFee;
            invalidExamFee.Id = invalidExamFeeId;

            var invalidExamFeeException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.Id),
                parameterValue: invalidExamFee.Id);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeException);

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
        public async Task ShouldThrowValidationExceptionOnModifyWhenExamIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidExamFeeId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee invalidExamFee = randomExamFee;
            invalidExamFee.ExamId = invalidExamFeeId;

            var invalidExamFeeException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.ExamId),
                parameterValue: invalidExamFee.ExamId);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeException);

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
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.CreatedBy = default;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.CreatedBy),
                parameterValue: inputExamFee.CreatedBy);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(inputExamFee);

            // then
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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.UpdatedBy = default;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.UpdatedBy),
                parameterValue: inputExamFee.UpdatedBy);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(inputExamFee);

            // then
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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.CreatedDate = default;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.CreatedDate),
                parameterValue: inputExamFee.CreatedDate);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(inputExamFee);

            // then
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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.UpdatedDate = default;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.UpdatedDate),
                parameterValue: inputExamFee.UpdatedDate);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(inputExamFee);

            // then
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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.UpdatedDate),
                parameterValue: inputExamFee.UpdatedDate);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(inputExamFee);

            // then
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
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.UpdatedBy = inputExamFee.CreatedBy;
            inputExamFee.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.UpdatedDate),
                parameterValue: inputExamFee.UpdatedDate);

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
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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
