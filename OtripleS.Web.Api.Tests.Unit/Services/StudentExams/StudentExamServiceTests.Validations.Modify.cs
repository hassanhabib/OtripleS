// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
    }
}
