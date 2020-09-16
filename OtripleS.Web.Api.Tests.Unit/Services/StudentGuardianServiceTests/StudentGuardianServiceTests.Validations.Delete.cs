using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardianServiceTests
{
   public partial class StudentGuardianServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStudentGuardianIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentGuardianId = default;
            Guid randomStudentId = default;
            Guid inputStudentGuardianId = randomStudentGuardianId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.GuardianId),
                parameterValue: inputStudentGuardianId
            );
            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            //when
            ValueTask<StudentGuardian> actualStudentGuardianDeleteTask =
                this.studentGuardianService.DeleteStudentGuardianAsync(inputStudentGuardianId, inputStudentId);

            //then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(
                () => actualStudentGuardianDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentGuardianId = Guid.NewGuid();
            Guid randomStudentId = default;
            Guid inputStudentGuardianId = randomStudentGuardianId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.StudentId),
                parameterValue: inputStudentId
            );
            var expectedStudentSemesterCourseValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            //when
            ValueTask<StudentGuardian> actualStudentGuardianDeleteTask =
                this.studentGuardianService.DeleteStudentGuardianAsync(inputStudentGuardianId, inputStudentId);

            //then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(
                () => actualStudentGuardianDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
