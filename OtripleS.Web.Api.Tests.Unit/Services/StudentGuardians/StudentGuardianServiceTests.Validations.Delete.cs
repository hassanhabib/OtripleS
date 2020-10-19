// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardians
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
                parameterValue: inputStudentId);

            var expectedStudentGuardianCourseValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            //when
            ValueTask<StudentGuardian> actualStudentGuardianDeleteTask =
                this.studentGuardianService.DeleteStudentGuardianAsync(inputStudentGuardianId, inputStudentId);

            //then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(
                () => actualStudentGuardianDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianCourseValidationException))),
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
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageStudentGuardianIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(randomDateTime);
            Guid inputStudentGuardianId = randomStudentGuardian.GuardianId;
            Guid inputStudentId = randomStudentGuardian.StudentId;
            StudentGuardian nullStorageStudentGuardian = null;

            var notFoundStudentGuardianException =
                new NotFoundStudentGuardianException(inputStudentGuardianId, inputStudentId);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(notFoundStudentGuardianException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId))
                    .ReturnsAsync(nullStorageStudentGuardian);
            // when
            ValueTask<StudentGuardian> actualStudentGuardianDeleteTask =
                this.studentGuardianService.DeleteStudentGuardianAsync(inputStudentGuardianId, inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                actualStudentGuardianDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
