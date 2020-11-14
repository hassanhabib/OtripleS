// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenExamIsNullAndLogItAsync()
        {
            //given
            Exam invalidExam = null;
            var nullExamException = new NullExamException();

            var expectedExamValidationException =
                new ExamValidationException(nullExamException);

            //when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(invalidExam);

            //then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenExamIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidExamId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(dateTime);
            Exam invalidExam = randomExam;
            invalidExam.Id = invalidExamId;

            var invalidExamException = new InvalidExamInputException(
                parameterName: nameof(Exam.Id),
                parameterValue: invalidExam.Id);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamException);

            //when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(invalidExam);

            //then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.CreatedBy = default;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.CreatedBy),
                parameterValue: inputExam.CreatedBy);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(It.IsAny<Guid>()),
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
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.UpdatedBy = default;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.UpdatedBy),
                parameterValue: inputExam.UpdatedBy);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(It.IsAny<Guid>()),
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
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.CreatedDate = default;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.CreatedDate),
                parameterValue: inputExam.CreatedDate);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(It.IsAny<Guid>()),
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
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.UpdatedDate = default;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.UpdatedDate),
                parameterValue: inputExam.UpdatedDate);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(It.IsAny<Guid>()),
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
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.UpdatedDate),
                parameterValue: inputExam.UpdatedDate);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(It.IsAny<Guid>()),
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
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.UpdatedBy = inputExam.CreatedBy;
            inputExam.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.UpdatedDate),
                parameterValue: inputExam.UpdatedDate);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfExamDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(dateTime);
            Exam nonExistentExam = randomExam;
            nonExistentExam.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Exam noExam = null;
            var notFoundExamException = new NotFoundExamException(nonExistentExam.Id);

            var expectedExamValidationException =
                new ExamValidationException(notFoundExamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(nonExistentExam.Id))
                    .ReturnsAsync(noExam);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(nonExistentExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(nonExistentExam.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(randomDate);
            Exam invalidExam = randomExam;
            invalidExam.UpdatedDate = randomDate;
            Exam storageExam = randomExam.DeepClone();
            Guid ExamId = invalidExam.Id;
            invalidExam.CreatedDate = storageExam.CreatedDate.AddMinutes(randomNumber);

            var invalidExamException = new InvalidExamInputException(
                parameterName: nameof(Exam.CreatedDate),
                parameterValue: invalidExam.CreatedDate);

            var expectedExamValidationException =
              new ExamValidationException(invalidExamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(ExamId))
                    .ReturnsAsync(storageExam);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(invalidExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(invalidExam.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDate = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(randomDate);
            Exam invalidExam = randomExam;
            invalidExam.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            Exam storageExam = randomExam.DeepClone();
            Guid ExamId = invalidExam.Id;
            invalidExam.CreatedBy = invalidCreatedBy;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.CreatedBy),
                parameterValue: invalidExam.CreatedBy);

            var expectedExamValidationException =
              new ExamValidationException(invalidExamInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(ExamId))
                    .ReturnsAsync(storageExam);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(invalidExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(invalidExam.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(randomDate);
            randomExam.CreatedDate = randomExam.CreatedDate.AddMinutes(minutesInThePast);
            Exam invalidExam = randomExam;
            invalidExam.UpdatedDate = randomDate;
            Exam storageExam = randomExam.DeepClone();
            Guid ExamId = invalidExam.Id;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.UpdatedDate),
                parameterValue: invalidExam.UpdatedDate);

            var expectedExamValidationException =
              new ExamValidationException(invalidExamInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(ExamId))
                    .ReturnsAsync(storageExam);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(invalidExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(invalidExam.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
