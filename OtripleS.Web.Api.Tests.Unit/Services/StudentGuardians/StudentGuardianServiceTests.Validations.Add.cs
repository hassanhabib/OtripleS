// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardians
{
    public partial class StudentGuardianServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentGuardianIsNullAndLogItAsync()
        {
            // given
            StudentGuardian randomStudentGuardian = default;
            StudentGuardian nullStudentGuardian = randomStudentGuardian;
            var nullStudentGuardianException = new NullStudentGuardianException();

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(nullStudentGuardianException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(nullStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.StudentId = default;

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.StudentId),
                parameterValue: inputStudentGuardian.StudentId);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.GuardianId = default;

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.GuardianId),
                parameterValue: inputStudentGuardian.GuardianId);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.CreatedBy = default;

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.CreatedBy),
                parameterValue: inputStudentGuardian.CreatedBy);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                 broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                     Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.CreatedDate = default;

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.CreatedDate),
                parameterValue: inputStudentGuardian.CreatedDate);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.UpdatedBy = default;

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.UpdatedBy),
                parameterValue: inputStudentGuardian.UpdatedBy);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.UpdatedDate = default;

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.UpdatedDate),
                parameterValue: inputStudentGuardian.UpdatedDate);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.UpdatedBy = Guid.NewGuid();

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.UpdatedBy),
                parameterValue: inputStudentGuardian.UpdatedBy);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.UpdatedBy = randomStudentGuardian.CreatedBy;
            inputStudentGuardian.UpdatedDate = GetRandomDateTime();

            var invalidStudentGuardianInputException = new InvalidStudentGuardianInputException(
                parameterName: nameof(StudentGuardian.UpdatedDate),
                parameterValue: inputStudentGuardian.UpdatedDate);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsNotRecentAndLogItAsync(
            int invalidMinutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.UpdatedBy = inputStudentGuardian.CreatedBy;
            inputStudentGuardian.CreatedDate = dateTime.AddMinutes(invalidMinutes);
            inputStudentGuardian.UpdatedDate = inputStudentGuardian.CreatedDate;

            var invalidStudentGuardianInputException =
                new InvalidStudentGuardianInputException(
                    parameterName: nameof(StudentGuardian.CreatedDate),
                    parameterValue: inputStudentGuardian.CreatedDate);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(invalidStudentGuardianInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<StudentGuardian> createStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                createStudentGuardianTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentGuardianAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian alreadyExistsStudentGuardian = randomStudentGuardian;
            alreadyExistsStudentGuardian.UpdatedBy = alreadyExistsStudentGuardian.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsStudentGuardianException =
                new AlreadyExistsStudentGuardianException(duplicateKeyException);

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(alreadyExistsStudentGuardianException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentGuardianAsync(alreadyExistsStudentGuardian))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<StudentGuardian> createStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(alreadyExistsStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                createStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(alreadyExistsStudentGuardian),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}