//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenExamFeeIsNullAndLogItAsync()
        {
            // given
            ExamFee randomExamFee = default;
            ExamFee nullExamFee = randomExamFee;
            var nullExamFeeException = new NullExamFeeException();

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(nullExamFeeException);

            // when
            ValueTask<ExamFee> addExamFeeTask =
                this.examFeeService.AddExamFeeAsync(nullExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                addExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenExamIdIsInvalidAndLogItAsync()
        {
            // given
            ExamFee randomExamFee = CreateRandomExamFee();
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.ExamId = default;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.ExamId),
                parameterValue: inputExamFee.ExamId);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> addExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                addExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenFeeIdIsInvalidAndLogItAsync()
        {
            // given
            ExamFee randomExamFee = CreateRandomExamFee();
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.FeeId = default;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.FeeId),
                parameterValue: inputExamFee.FeeId);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> addExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                addExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenExamFeeAlreadyExistsAndLogItAsync()
        {
            // given
            ExamFee randomExamFee = CreateRandomExamFee();
            ExamFee alreadyExistsExamFee = randomExamFee;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsExamFeeException =
                new AlreadyExistsExamFeeException(duplicateKeyException);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(alreadyExistsExamFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamFeeAsync(alreadyExistsExamFee))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ExamFee> addExamFeeTask =
                this.examFeeService.AddExamFeeAsync(alreadyExistsExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                addExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(alreadyExistsExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            ExamFee randomExamFee = CreateRandomExamFee();
            ExamFee invalidExamFee = randomExamFee;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidExamFeeReferenceException =
                new InvalidExamFeeReferenceException(foreignKeyConstraintConflictException);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamFeeAsync(invalidExamFee))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<ExamFee> addExamFeeTask =
                this.examFeeService.AddExamFeeAsync(invalidExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                addExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(invalidExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedByIsInvalidAndLogItAsync()
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
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                createExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsInvalidAndLogItAsync()
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
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                createExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsInvalidAndLogItAsync()
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
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                createExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsInvalidAndLogItAsync()
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
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                createExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.UpdatedBy = Guid.NewGuid();

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.UpdatedBy),
                parameterValue: inputExamFee.UpdatedBy);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                createExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.UpdatedBy = randomExamFee.CreatedBy;
            inputExamFee.UpdatedDate = GetRandomDateTime();

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.UpdatedDate),
                parameterValue: inputExamFee.UpdatedDate);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                createExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
