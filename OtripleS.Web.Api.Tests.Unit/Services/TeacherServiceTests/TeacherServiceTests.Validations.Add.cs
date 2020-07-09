// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherServiceTests
{
    public partial class TeacherServiceTests
    {
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
            ValueTask<Teacher> registerTeacherTask =
                this.teacherService.CreateTeacherAsync(nullTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                registerTeacherTask.AsTask());

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
            ValueTask<Teacher> registerTeacherTask =
                this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                registerTeacherTask.AsTask());

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
            ValueTask<Teacher> registerTeacherTask =
                this.teacherService.CreateTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                registerTeacherTask.AsTask());

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
            ValueTask<Teacher> registerTeacherTask =
                this.teacherService.CreateTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                registerTeacherTask.AsTask());

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
            ValueTask<Teacher> registerTeacherTask =
                this.teacherService.CreateTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                registerTeacherTask.AsTask());

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
            ValueTask<Teacher> registerTeacherTask =
                this.teacherService.CreateTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                registerTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenGenderIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            inputTeacher.Gender = default;

            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.Gender),
                parameterValue: inputTeacher.Gender);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> registerTeacherTask =
                this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                registerTeacherTask.AsTask());

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
    }
}
