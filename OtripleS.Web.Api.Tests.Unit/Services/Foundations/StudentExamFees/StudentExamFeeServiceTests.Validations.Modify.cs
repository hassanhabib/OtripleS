// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenStudentExamFeeIsNullAndLogItAsync()
        {
            //given
            StudentExamFee invalidStudentExamFee = null;
            var nullStudentExamFeeException = new NullStudentExamFeeException();

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(nullStudentExamFeeException);

            //when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(invalidStudentExamFee);

            //then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenStudentIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidStudentId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee invalidStudentExamFee = randomStudentExamFee;
            invalidStudentExamFee.StudentId = invalidStudentId;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.StudentId),
                parameterValue: invalidStudentExamFee.StudentId);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            //when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(invalidStudentExamFee);

            //then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenExamFeeIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidExamId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee invalidStudentExamFee = randomStudentExamFee;
            invalidStudentExamFee.ExamFeeId = invalidExamId;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.ExamFeeId),
                parameterValue: invalidStudentExamFee.ExamFeeId);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            //when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(invalidStudentExamFee);

            //then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
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
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.CreatedBy = default;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.CreatedBy),
                parameterValue: inputStudentExamFee.CreatedBy);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
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
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedBy = default;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.UpdatedBy),
                parameterValue: inputStudentExamFee.UpdatedBy);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
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
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.CreatedDate = default;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.CreatedDate),
                parameterValue: inputStudentExamFee.CreatedDate);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
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
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedDate = default;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.UpdatedDate),
                parameterValue: inputStudentExamFee.UpdatedDate);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.UpdatedDate),
                parameterValue: inputStudentExamFee.UpdatedDate);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
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
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedBy = inputStudentExamFee.CreatedBy;
            inputStudentExamFee.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.UpdatedDate),
                parameterValue: inputStudentExamFee.UpdatedDate);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStudentExamFeeDoesnotExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee nonExistentStudentExamFee = randomStudentExamFee;
            nonExistentStudentExamFee.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            StudentExamFee noStudentExamFee = null;

            var notFoundStudentExamFeeException =
                new NotFoundStudentExamFeeException(
                    nonExistentStudentExamFee.StudentId,
                    nonExistentStudentExamFee.ExamFeeId);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(notFoundStudentExamFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    nonExistentStudentExamFee.StudentId,
                    nonExistentStudentExamFee.ExamFeeId))
                        .ReturnsAsync(noStudentExamFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(nonExistentStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    nonExistentStudentExamFee.StudentId,
                    nonExistentStudentExamFee.ExamFeeId),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
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
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(randomDate);
            StudentExamFee invalidStudentExamFee = randomStudentExamFee;
            invalidStudentExamFee.UpdatedDate = randomDate;
            StudentExamFee storageStudentExamFee = randomStudentExamFee.DeepClone();
            Guid studentId = invalidStudentExamFee.StudentId;
            Guid semesterCourseId = invalidStudentExamFee.ExamFeeId;
            invalidStudentExamFee.CreatedDate = storageStudentExamFee.CreatedDate.AddMinutes(randomNumber);

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.CreatedDate),
                parameterValue: invalidStudentExamFee.CreatedDate);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    invalidStudentExamFee.StudentId,
                    invalidStudentExamFee.ExamFeeId))
                        .ReturnsAsync(storageStudentExamFee);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(invalidStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    invalidStudentExamFee.StudentId,
                    invalidStudentExamFee.ExamFeeId),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
