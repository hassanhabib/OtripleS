using System;
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
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenStudentExamFeeIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentExamFeeId = default;
            Guid inputStudentExamFeeId = randomStudentExamFeeId;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.Id),
                parameterValue: inputStudentExamFeeId);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.studentExamFeeService.RemoveStudentExamFeeByIdAsync(inputStudentExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageStudentExamFeeIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(randomDateTime);
            Guid inputStudentExamFeeId = randomStudentExamFee.Id;
            StudentExamFee nullStorageStudentExamFee = null;

            var notFoundStudentExamFeeException =
                new NotFoundStudentExamFeeException(inputStudentExamFeeId);

            var expectedAssignmentValidationException =
                new StudentExamFeeValidationException(notFoundStudentExamFeeException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentExamFeeByIdAsync(inputStudentExamFeeId))
                    .ReturnsAsync(nullStorageStudentExamFee);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.studentExamFeeService.RemoveStudentExamFeeByIdAsync(inputStudentExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(It.IsAny<Guid>()),
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
