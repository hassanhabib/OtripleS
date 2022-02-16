// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentRegistrationIsNullAndLogItAsync()
        {
            // given
            StudentRegistration invalidStudentRegistration = null;
            var nullStudentRegistrationException = new NullStudentRegistrationException();

            var expectedStudentRegistrationValidationException =
                new StudentRegistrationValidationException(nullStudentRegistrationException);

            // when
            ValueTask<StudentRegistration> addStudentRegistrationTask =
                this.studentRegistrationService.AddStudentRegistrationAsync(invalidStudentRegistration);

            // then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(() =>
                addStudentRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistration();
            StudentRegistration inputStudentRegistration = randomStudentRegistration;
            inputStudentRegistration.StudentId = default;

            var invalidStudentRegistrationInputException = new InvalidStudentRegistrationException(
                parameterName: nameof(StudentRegistration.StudentId),
                parameterValue: inputStudentRegistration.StudentId);

            var expectedStudentRegistrationValidationException =
                new StudentRegistrationValidationException(invalidStudentRegistrationInputException);

            // when
            ValueTask<StudentRegistration> addStudentRegistrationTask =
                this.studentRegistrationService.AddStudentRegistrationAsync(inputStudentRegistration);

            // then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(() =>
                addStudentRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenRegistrationIdIsInvalidAndLogItAsync()
        {
            // given
            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistration();
            StudentRegistration inputStudentRegistration = randomStudentRegistration;
            inputStudentRegistration.RegistrationId = default;

            var invalidStudentRegistrationInputException = new InvalidStudentRegistrationException(
                parameterName: nameof(StudentRegistration.RegistrationId),
                parameterValue: inputStudentRegistration.RegistrationId);

            var expectedStudentRegistrationValidationException =
                new StudentRegistrationValidationException(invalidStudentRegistrationInputException);

            // when
            ValueTask<StudentRegistration> addStudentRegistrationTask =
                this.studentRegistrationService.AddStudentRegistrationAsync(inputStudentRegistration);

            // then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(() =>
                addStudentRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentRegistrationAlreadyExistsAndLogItAsync()
        {
            // given
            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistration();
            StudentRegistration alreadyExistsStudentRegistration = randomStudentRegistration;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsStudentRegistrationException =
                new AlreadyExistsStudentRegistrationException(duplicateKeyException);

            var expectedStudentRegistrationValidationException =
                new StudentRegistrationValidationException(alreadyExistsStudentRegistrationException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentRegistrationAsync(alreadyExistsStudentRegistration))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<StudentRegistration> addStudentRegistrationTask =
                this.studentRegistrationService.AddStudentRegistrationAsync(alreadyExistsStudentRegistration);

            // then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(() =>
                addStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedStudentRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistration();
            StudentRegistration someStudentRegistration = randomStudentRegistration;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidStudentRegistrationReferenceException =
                new InvalidStudentRegistrationReferenceException(foreignKeyConstraintConflictException);

            var expectedStudentRegistrationValidationException =
                new StudentRegistrationValidationException(invalidStudentRegistrationReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentRegistrationAsync(someStudentRegistration))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<StudentRegistration> addStudentRegistrationTask =
                this.studentRegistrationService.AddStudentRegistrationAsync(someStudentRegistration);

            // then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(() =>
                addStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
