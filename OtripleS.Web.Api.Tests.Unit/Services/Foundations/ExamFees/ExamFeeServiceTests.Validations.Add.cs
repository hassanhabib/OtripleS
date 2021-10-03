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

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamFees
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
        public async void ShouldThrowValidationExceptionOnAddWhenIdsAreInvalidAndLogItAsync()
        {
            // given
            ExamFee randomExamFee = CreateRandomExamFee();
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.ExamId = default;
            inputExamFee.FeeId = default;

            var invalidExamFeeInputException = new InvalidExamFeeException();

            invalidExamFeeInputException.AddData(
                key: nameof(ExamFee.ExamId),
                values: "Id is required");

            invalidExamFeeInputException.AddData(
                key: nameof(ExamFee.FeeId),
                values: "Id is required");

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> addExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                addExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAuditFieldsAreInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.CreatedBy = default;
            inputExamFee.CreatedDate = default;
            inputExamFee.UpdatedBy = default;
            inputExamFee.UpdatedDate = default;

            var invalidExamFeeInputException = new InvalidExamFeeException();

            invalidExamFeeInputException.AddData(
                key: nameof(ExamFee.CreatedBy),
                values: "Id is required");

            invalidExamFeeInputException.AddData(
                key: nameof(ExamFee.CreatedDate),
                values: "Date is required");

            invalidExamFeeInputException.AddData(
                key: nameof(ExamFee.UpdatedBy),
                values: "Id is required");

            invalidExamFeeInputException.AddData(
                key: nameof(ExamFee.UpdatedDate),
                values: "Date is required");

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                createExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(expectedExamFeeValidationException))),
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

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee inputExamFee = randomExamFee;
            inputExamFee.UpdatedBy = inputExamFee.CreatedBy;
            inputExamFee.CreatedDate = dateTime.AddMinutes(minutes);
            inputExamFee.UpdatedDate = inputExamFee.CreatedDate;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.CreatedDate),
                parameterValue: inputExamFee.CreatedDate);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                createExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

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
        public async void ShouldThrowValidationExceptionOnCreateWhenExamFeeAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            ExamFee alreadyExistsExamFee = randomExamFee;
            alreadyExistsExamFee.UpdatedBy = alreadyExistsExamFee.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsExamFeeException =
                new AlreadyExistsExamFeeException(duplicateKeyException);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(alreadyExistsExamFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamFeeAsync(alreadyExistsExamFee))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(alreadyExistsExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                createExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(alreadyExistsExamFee),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            randomExamFee.UpdatedBy = randomExamFee.CreatedBy;
            randomExamFee.UpdatedDate = randomExamFee.CreatedDate;
            ExamFee invalidExamFee = randomExamFee;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidExamFeeReferenceException =
                new InvalidExamFeeReferenceException(foreignKeyConstraintConflictException);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamFeeAsync(invalidExamFee))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<ExamFee> addExamFeeTask =
                this.examFeeService.AddExamFeeAsync(invalidExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                addExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(invalidExamFee),
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
