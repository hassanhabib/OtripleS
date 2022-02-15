// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenStudentExamFeeIsNullAndLogItAsync()
        {
            // given
            StudentExamFee invalidStudentExamFee = null;

            var nullStudentExamFeeException = new NullStudentExamFeeException();

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(nullStudentExamFeeException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(invalidStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.StudentId = default;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.StudentId),
                parameterValue: inputStudentExamFee.StudentId);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> registerStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                registerStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenExamFeeIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.ExamFeeId = default;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.ExamFeeId),
                parameterValue: inputStudentExamFee.ExamFeeId);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> registerStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                registerStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedByInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.CreatedBy = default;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.CreatedBy),
                parameterValue: inputStudentExamFee.CreatedBy);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedBy = default;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.UpdatedBy),
                parameterValue: inputStudentExamFee.UpdatedBy);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.CreatedDate = default;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.CreatedDate),
                parameterValue: inputStudentExamFee.CreatedDate);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedDate = default;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.UpdatedDate),
                parameterValue: inputStudentExamFee.UpdatedDate);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedBy = Guid.NewGuid();

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.UpdatedBy),
                parameterValue: inputStudentExamFee.UpdatedBy);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedBy = randomStudentExamFee.CreatedBy;
            inputStudentExamFee.UpdatedDate = GetRandomDateTime();

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.UpdatedDate),
                parameterValue: inputStudentExamFee.UpdatedDate);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedBy = inputStudentExamFee.CreatedBy;
            inputStudentExamFee.CreatedDate = dateTime.AddMinutes(minutes);
            inputStudentExamFee.UpdatedDate = inputStudentExamFee.CreatedDate;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.CreatedDate),
                parameterValue: inputStudentExamFee.CreatedDate);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                createStudentExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenStudentExamFeeAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee alreadyExistsStudentExamFee = randomStudentExamFee;
            alreadyExistsStudentExamFee.UpdatedBy = alreadyExistsStudentExamFee.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsStudentExamFeeException =
                new AlreadyExistsStudentExamFeeException(duplicateKeyException);

            var expectedExamFeeValidationException =
                new StudentExamFeeValidationException(alreadyExistsStudentExamFeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamFeeAsync(alreadyExistsStudentExamFee))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(alreadyExistsStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                createStudentExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamFeeAsync(alreadyExistsStudentExamFee),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedExamFeeValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
