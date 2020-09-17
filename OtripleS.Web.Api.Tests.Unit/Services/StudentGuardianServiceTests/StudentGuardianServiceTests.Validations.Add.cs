// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardianServiceTests
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
            ValueTask<StudentGuardian> createStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(nullStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                createStudentGuardianTask.AsTask());

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
            ValueTask<StudentGuardian> createStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                createStudentGuardianTask.AsTask());

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
            ValueTask<StudentGuardian> createStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                createStudentGuardianTask.AsTask());

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
            ValueTask<StudentGuardian> createStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                createStudentGuardianTask.AsTask());

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
            ValueTask<StudentGuardian> createStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                createStudentGuardianTask.AsTask());

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
            ValueTask<StudentGuardian> createStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                createStudentGuardianTask.AsTask());

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
            ValueTask<StudentGuardian> createStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                createStudentGuardianTask.AsTask());

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
    }
}