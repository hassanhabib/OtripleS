// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenStudentExamIsNullAndLogItAsync()
        {
            //given
            StudentExam invalidStudentExam = null;
            var nullStudentExamException = new NullStudentExamException();

            var expectedStudentExamValidationException =
                new StudentExamValidationException(nullStudentExamException);

            //when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(invalidStudentExam);

            //then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam invalidStudentExam = randomStudentExam;
            invalidStudentExam.Id = invalidId;

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.Id),
                parameterValue: invalidStudentExam.Id);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            //when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(invalidStudentExam);

            //then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam invalidStudentExam = randomStudentExam;
            invalidStudentExam.StudentId = invalidStudentId;

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.StudentId),
                parameterValue: invalidStudentExam.StudentId);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            //when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(invalidStudentExam);

            //then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam invalidStudentExam = randomStudentExam;
            invalidStudentExam.ExamId = invalidExamId;

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.ExamId),
                parameterValue: invalidStudentExam.ExamId);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            //when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(invalidStudentExam);

            //then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam inputStudentExam = randomStudentExam;
            inputStudentExam.CreatedBy = default;

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.CreatedBy),
                parameterValue: inputStudentExam.CreatedBy);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(inputStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam inputStudentExam = randomStudentExam;
            inputStudentExam.UpdatedBy = default;

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.UpdatedBy),
                parameterValue: inputStudentExam.UpdatedBy);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(inputStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam inputStudentExam = randomStudentExam;
            inputStudentExam.CreatedDate = default;

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.CreatedDate),
                parameterValue: inputStudentExam.CreatedDate);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(inputStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam inputStudentExam = randomStudentExam;
            inputStudentExam.UpdatedDate = default;

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.UpdatedDate),
                parameterValue: inputStudentExam.UpdatedDate);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(inputStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam inputStudentExam = randomStudentExam;

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.UpdatedDate),
                parameterValue: inputStudentExam.UpdatedDate);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(inputStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam inputStudentExam = randomStudentExam;
            inputStudentExam.UpdatedBy = inputStudentExam.CreatedBy;
            inputStudentExam.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.UpdatedDate),
                parameterValue: inputStudentExam.UpdatedDate);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(inputStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStudentExamDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam nonExistentStudentExam = randomStudentExam;
            nonExistentStudentExam.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            StudentExam noStudentExam = null;
            var notFoundStudentExamException = new NotFoundStudentExamException(nonExistentStudentExam.Id);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(notFoundStudentExamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(nonExistentStudentExam.Id))
                    .ReturnsAsync(noStudentExam);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(nonExistentStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(nonExistentStudentExam.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(randomDate);
            StudentExam invalidStudentExam = randomStudentExam;
            invalidStudentExam.UpdatedDate = randomDate;
            StudentExam storageStudentExam = randomStudentExam.DeepClone();
            Guid studentId = invalidStudentExam.StudentId;
            Guid semesterCourseId = invalidStudentExam.ExamId;
            invalidStudentExam.CreatedDate = storageStudentExam.CreatedDate.AddMinutes(randomNumber);

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.CreatedDate),
                parameterValue: invalidStudentExam.CreatedDate);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(invalidStudentExam.Id))
                    .ReturnsAsync(storageStudentExam);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(invalidStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(invalidStudentExam.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
