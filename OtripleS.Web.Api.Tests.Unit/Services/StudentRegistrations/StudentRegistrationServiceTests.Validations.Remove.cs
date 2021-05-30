// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentId = default;
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid inputRegistrationId = randomRegistrationId;

            var invalidStudentRegistrationException = new InvalidStudentRegistrationException(
                parameterName: nameof(StudentRegistration.StudentId),
                parameterValue: inputStudentId);

            var expectedStudentRegistrationCourseValidationException =
                new StudentRegistrationValidationException(invalidStudentRegistrationException);

            //when
            ValueTask<StudentRegistration> actualStudentRegistrationDeleteTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputStudentId,
                    inputRegistrationId);

            //then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(() => 
                actualStudentRegistrationDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStudentRegistrationIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid randomRegistrationId = default;
            Guid inputStudentId = randomStudentId;
            Guid inputRegistrationId = randomRegistrationId;

            var invalidStudentRegistrationException = new InvalidStudentRegistrationException(
                parameterName: nameof(StudentRegistration.RegistrationId),
                parameterValue: inputRegistrationId);

            var expectedStudentRegistrationValidationException =
                new StudentRegistrationValidationException(invalidStudentRegistrationException);

            // when
            ValueTask<StudentRegistration> actualStudentRegistrationDeleteTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputStudentId,
                    inputRegistrationId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(() => 
                actualStudentRegistrationDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();            
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageStudentRegistrationIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistration(randomDateTime);
            Guid inputStudentRegistrationId = randomStudentRegistration.RegistrationId;
            Guid inputStudentId = randomStudentRegistration.StudentId;
            StudentRegistration nullStorageStudentRegistration = null;

            var notFoundStudentRegistrationException =
                new NotFoundStudentRegistrationException(inputStudentRegistrationId, inputStudentId);

            var expectedStudentRegistrationValidationException =
                new StudentRegistrationValidationException(notFoundStudentRegistrationException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentRegistrationByIdAsync(inputStudentRegistrationId, inputStudentId))
                    .ReturnsAsync(nullStorageStudentRegistration);
            // when
            ValueTask<StudentRegistration> actualStudentRegistrationDeleteTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputStudentRegistrationId, 
                    inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(() =>
                actualStudentRegistrationDeleteTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentRegistrationId, inputStudentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
