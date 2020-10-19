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
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomGuardianId = Guid.NewGuid();
            Guid randomStudentId = default;
            Guid inputGuardianId = randomGuardianId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.StudentId),
                parameterValue: inputStudentId);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            // when
            ValueTask<StudentGuardian> actualStudentGuardianTask =
                this.studentGuardianService.RetrieveStudentGuardianByIdAsync(inputStudentId, inputGuardianId);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() => actualStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenGuardianIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomGuardianId = default;
            Guid randomStudentId = Guid.NewGuid();
            Guid inputGuardianId = randomGuardianId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.GuardianId),
                parameterValue: inputGuardianId);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            // when
            ValueTask<StudentGuardian> actualStudentGuardianTask =
                this.studentGuardianService.RetrieveStudentGuardianByIdAsync(inputStudentId, inputGuardianId);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() => actualStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageStudentGuardianIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(randomDateTime);
            Guid inputGuardianId = randomStudentGuardian.GuardianId;
            Guid inputStudentId = randomStudentGuardian.StudentId;
            StudentGuardian nullStorageStudentGuardian = null;

            var notFoundStudentGuardianException =
                new NotFoundStudentGuardianException(inputStudentId, inputGuardianId);

            var expectedSemesterCourseValidationException =
                new StudentGuardianValidationException(notFoundStudentGuardianException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentGuardianByIdAsync(inputStudentId, inputGuardianId))
                    .ReturnsAsync(nullStorageStudentGuardian);

            // when
            ValueTask<StudentGuardian> actualStudentGuardianRetrieveTask =
                this.studentGuardianService.RetrieveStudentGuardianByIdAsync(inputStudentId, inputGuardianId);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                actualStudentGuardianRetrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
