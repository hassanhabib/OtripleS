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

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
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
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeValidationException))),
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
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee invalidStudentExamFee = randomStudentExamFee;
            invalidStudentExamFee.Id = invalidId;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.Id),
                parameterValue: invalidStudentExamFee.Id);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            //when
            ValueTask<StudentExamFee> modifyStudentExamFeeTask =
                this.studentExamFeeService.ModifyStudentExamFeeAsync(invalidStudentExamFee);

            //then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                modifyStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
