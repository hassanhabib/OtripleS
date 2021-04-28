//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentId = default;
            Guid inputStudentId = randomStudentId;
            Guid randomExamFeeId = Guid.NewGuid();
            Guid inputExamFeeId = randomExamFeeId;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.StudentId),
                parameterValue: inputStudentId);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.studentExamFeeService.RemoveStudentExamFeeByIdAsync(
                    inputStudentId,
                    inputExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();            
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenExamFeeIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();            
            Guid inputStudentId = randomStudentId;
            Guid randomExamFeeId = default;
            Guid inputExamFeeId = randomExamFeeId;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.ExamFeeId),
                parameterValue: inputExamFeeId);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.studentExamFeeService.RemoveStudentExamFeeByIdAsync(
                    inputStudentId,
                    inputExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageStudentExamFeeIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(randomDateTime);
            Guid inputStudentId = randomStudentExamFee.StudentId;
            Guid inputExamFeeId = randomStudentExamFee.ExamFeeId;
            StudentExamFee nullStudentExamFees = null;

            var notFoundStudentExamFeeException =
                new NotFoundStudentExamFeeException(inputStudentId, inputExamFeeId);

            var expectedAssignmentValidationException =
                new StudentExamFeeValidationException(notFoundStudentExamFeeException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()))
                        .ReturnsAsync(nullStudentExamFees);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.studentExamFeeService.RemoveStudentExamFeeByIdAsync(
                    inputStudentId,
                    inputExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
