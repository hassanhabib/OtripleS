// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Teachers
{
    public partial class TeacherServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnCreateWhenTeacherUserIdIsInvalidAndLogItAsync(
            string invalidTeacherUserId)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher invalidTeacher = randomTeacher;
            invalidTeacher.UserId = invalidTeacherUserId;

            var invalidTeacherException = new InvalidTeacherInputException(
               parameterName: nameof(Teacher.UserId),
               parameterValue: invalidTeacher.UserId);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnCreateWhenTeacherFirstNameIsInvalidAndLogItAsync(
            string invalidTeacherFirstName)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher invalidTeacher = randomTeacher;
            invalidTeacher.FirstName = invalidTeacherFirstName;

            var invalidTeacherException = new InvalidTeacherInputException(
               parameterName: nameof(Teacher.FirstName),
               parameterValue: invalidTeacher.FirstName);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnCreateWhenTeacherEmployeeNumberIsInvalidAndLogItAsync(
            string invalidTeacherEmployeeNumber)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher invalidTeacher = randomTeacher;
            invalidTeacher.EmployeeNumber = invalidTeacherEmployeeNumber;

            var invalidTeacherException = new InvalidTeacherInputException(
               parameterName: nameof(Teacher.EmployeeNumber),
               parameterValue: invalidTeacher.EmployeeNumber);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnCreateWhenTeacherLastNameIsInvalidAndLogItAsync(
            string invalidTeacherLastName)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher invalidTeacher = randomTeacher;
            invalidTeacher.LastName = invalidTeacherLastName;

            var invalidTeacherException = new InvalidTeacherInputException(
               parameterName: nameof(Teacher.LastName),
               parameterValue: invalidTeacher.LastName);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenTeacherIsNullAndLogItAsync()
        {
            // given
            Teacher randomTeacher = null;
            Teacher nullTeacher = randomTeacher;
            var nullTeacherException = new NullTeacherException();

            var expectedTeacherValidationException =
                new TeacherValidationException(nullTeacherException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(nullTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            inputTeacher.Id = default;

            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.Id),
                parameterValue: inputTeacher.Id);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            inputTeacher.CreatedBy = default;

            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.CreatedBy),
                parameterValue: inputTeacher.CreatedBy);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            inputTeacher.CreatedDate = default;

            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.CreatedDate),
                parameterValue: inputTeacher.CreatedDate);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            inputTeacher.UpdatedBy = default;

            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.UpdatedBy),
                parameterValue: inputTeacher.UpdatedBy);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            inputTeacher.UpdatedDate = default;

            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.UpdatedDate),
                parameterValue: inputTeacher.UpdatedDate);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            inputTeacher.UpdatedBy = randomTeacher.CreatedBy;
            inputTeacher.UpdatedDate = GetRandomDateTime();

            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.UpdatedDate),
                parameterValue: inputTeacher.UpdatedDate);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            inputTeacher.UpdatedBy = Guid.NewGuid();

            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.UpdatedBy),
                parameterValue: inputTeacher.UpdatedBy);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            inputTeacher.UpdatedBy = inputTeacher.CreatedBy;
            inputTeacher.CreatedDate = dateTime.AddMinutes(minutes);
            inputTeacher.UpdatedDate = inputTeacher.CreatedDate;

            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.CreatedDate),
                parameterValue: inputTeacher.CreatedDate);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenTeacherAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher alreadyExistsTeacher = randomTeacher;
            alreadyExistsTeacher.UpdatedBy = alreadyExistsTeacher.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsTeacherException =
                new AlreadyExistsTeacherException(duplicateKeyException);

            var expectedTeacherValidationException =
                new TeacherValidationException(alreadyExistsTeacherException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherAsync(alreadyExistsTeacher))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Teacher> registerTeacherTask =
                this.teacherService.CreateTeacherAsync(alreadyExistsTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                registerTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAsync(alreadyExistsTeacher),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
