// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenExamIsNullAndLogItAsync()
        {
            // given
            Exam randomExam = default;
            Exam nullExam = randomExam;
            var nullExamException = new NullExamException();

            var expectedExamValidationException =
                new ExamValidationException(nullExamException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(nullExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(It.IsAny<Exam>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.Id = default;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.Id),
                parameterValue: inputExam.Id);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(It.IsAny<Exam>()),
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
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.CreatedBy = default;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.CreatedBy),
                parameterValue: inputExam.CreatedBy);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                 broker.InsertExamAsync(It.IsAny<Exam>()),
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
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.CreatedDate = default;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.CreatedDate),
                parameterValue: inputExam.CreatedDate);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(It.IsAny<Exam>()),
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
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.UpdatedBy = default;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.UpdatedBy),
                parameterValue: inputExam.UpdatedBy);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(It.IsAny<Exam>()),
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
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.UpdatedDate = default;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.UpdatedDate),
                parameterValue: inputExam.UpdatedDate);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(It.IsAny<Exam>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.UpdatedBy = Guid.NewGuid();

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.UpdatedBy),
                parameterValue: inputExam.UpdatedBy);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(It.IsAny<Exam>()),
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
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.UpdatedBy = randomExam.CreatedBy;
            inputExam.UpdatedDate = GetRandomDateTime();

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.UpdatedDate),
                parameterValue: inputExam.UpdatedDate);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(It.IsAny<Exam>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenExamTypeIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.Type = GetInValidExamType();

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.Type),
                parameterValue: inputExam.Type);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(It.IsAny<Exam>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsNotRecentAndLogItAsync(
           int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(dateTime);
            Exam inputExam = randomExam;
            inputExam.UpdatedBy = inputExam.CreatedBy;
            inputExam.CreatedDate = dateTime.AddMinutes(minutes);
            inputExam.UpdatedDate = inputExam.CreatedDate;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.CreatedDate),
                parameterValue: inputExam.CreatedDate);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(inputExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(It.IsAny<Exam>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenExamAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(dateTime);
            Exam alreadyExistsExam = randomExam;
            alreadyExistsExam.UpdatedBy = alreadyExistsExam.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsExamException =
                new AlreadyExistsExamException(duplicateKeyException);

            var expectedExamValidationException =
                new ExamValidationException(alreadyExistsExamException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamAsync(alreadyExistsExam))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(alreadyExistsExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(alreadyExistsExam),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
