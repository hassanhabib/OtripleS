// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherServiceTests
{
    public partial class TeacherServiceTests
    {
        [Fact]
        public async Task ShouldThorwValidationExceptionOnModifyWhenTeacherIsNullAndLogItAsync()
        {
            // given
            Teacher invalidTeacher = null;
            var nullTeacherException = new NullTeacherException();

            var expectedTeacherValidationException =
                new TeacherValidationException(nullTeacherException);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenTeacherIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidTeacherId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher invalidTeacher = randomTeacher;
            invalidTeacher.Id = invalidTeacherId;

            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.Id),
                parameterValue: invalidTeacher.Id);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnModifyWhenTeacherUserIdIsInvalidAndLogItAsync(
            string invalidTeacherUserId)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher invalidTeacher = randomTeacher;
            invalidTeacher.UserId = invalidTeacherUserId;

            var invalidTeacherInputException = new InvalidTeacherInputException(
               parameterName: nameof(Teacher.UserId),
               parameterValue: invalidTeacher.UserId);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnModifyWhenTeacherFirstNameIsInvalidAndLogItAsync(
            string invalidTeacherFirstName)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher invalidTeacher = randomTeacher;
            invalidTeacher.FirstName = invalidTeacherFirstName;

            var invalidTeacherInputException = new InvalidTeacherInputException(
               parameterName: nameof(Teacher.FirstName),
               parameterValue: invalidTeacher.FirstName);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnModifyWhenTeacherEmployeeNumberIsInvalidAndLogItAsync(
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
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnModifyWhenTeacherLastNameIsInvalidAndLogItAsync(
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
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
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
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
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
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            
            var invalidTeacherInputException = new InvalidTeacherInputException(
                parameterName: nameof(Teacher.UpdatedDate),
                parameterValue: inputTeacher.UpdatedDate);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherInputException);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

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
