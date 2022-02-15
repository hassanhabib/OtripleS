// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Teachers
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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAsync(It.IsAny<Teacher>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnModifyIfTeacherIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidTeacher = new Teacher
            {
                UserId = invalidText,
                EmployeeNumber = invalidText,
                FirstName = invalidText,
                MiddleName = invalidText,
                LastName = invalidText
            };

            var invalidTeacherException = new InvalidTeacherException();

            invalidTeacherException.AddData(
                key: nameof(Teacher.Id),
                values: "Id is required");

            invalidTeacherException.AddData(
                key: nameof(Teacher.UserId),
                values: "Text is required");

            invalidTeacherException.AddData(
                key: nameof(Teacher.EmployeeNumber),
                values: "Text is required");

            invalidTeacherException.AddData(
                key: nameof(Teacher.FirstName),
                values: "Text is required");

            invalidTeacherException.AddData(
                key: nameof(Teacher.MiddleName),
                values: "Text is required");

            invalidTeacherException.AddData(
                key: nameof(Teacher.LastName),
                values: "Text is required");

            invalidTeacherException.AddData(
                key: nameof(Teacher.CreatedDate),
                values: "Date is required");

            invalidTeacherException.AddData(
                key: nameof(Teacher.UpdatedDate),
                values: "Date is required");

            invalidTeacherException.AddData(
                key: nameof(Teacher.CreatedBy),
                values: "Id is required");

            invalidTeacherException.AddData(
                key: nameof(Teacher.UpdatedBy),
                values: "Id is required");

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherException);

            // when
            ValueTask<Teacher> createTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                createTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedTeacherValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAsync(It.IsAny<Teacher>()),
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
            Teacher invalidTeacher = randomTeacher;
            var invalidTeacherException = new InvalidTeacherException();

            invalidTeacherException.AddData(
                key: nameof(Teacher.UpdatedDate),
                values: $"UpdatedDate is the same as {nameof(Teacher.CreatedDate)}");

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherException);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherValidationException))),
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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher inputTeacher = randomTeacher;
            inputTeacher.UpdatedBy = inputTeacher.CreatedBy;
            inputTeacher.UpdatedDate = dateTime.AddMinutes(minutes);
            var invalidTeacherException = new InvalidTeacherException();

            invalidTeacherException.AddData(
                key: nameof(Teacher.UpdatedDate),
                values: $"Date is not recent");

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(inputTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThorwValidationExceptionOnModifyIfTeacherDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Teacher nonExistentTeacher = randomTeacher;
            nonExistentTeacher.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Teacher noTeacher = null;
            var notFoundTeacherException = new NotFoundTeacherException(nonExistentTeacher.Id);

            var expectedTeacherValidationException =
                new TeacherValidationException(notFoundTeacherException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(nonExistentTeacher.Id))
                    .ReturnsAsync(noTeacher);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(nonExistentTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(nonExistentTeacher.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(randomDate);
            Teacher invalidTeacher = randomTeacher;
            invalidTeacher.UpdatedDate = randomDate;
            Teacher storageTeacher = randomTeacher.DeepClone();
            Guid studentId = invalidTeacher.Id;
            invalidTeacher.CreatedDate = storageTeacher.CreatedDate.AddMinutes(randomNumber);

            var invalidTeacherException = new InvalidTeacherException(
                parameterName: nameof(Teacher.CreatedDate),
                parameterValue: invalidTeacher.CreatedDate);

            var expectedTeacherValidationException =
              new TeacherValidationException(invalidTeacherException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(studentId))
                    .ReturnsAsync(storageTeacher);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(invalidTeacher.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDate = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(randomDate);
            Teacher invalidTeacher = randomTeacher;
            invalidTeacher.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            Teacher storageTeacher = randomTeacher.DeepClone();
            Guid studentId = invalidTeacher.Id;
            invalidTeacher.CreatedBy = invalidCreatedBy;

            var InvalidTeacherException = new InvalidTeacherException(
                parameterName: nameof(Teacher.CreatedBy),
                parameterValue: invalidTeacher.CreatedBy);

            var expectedTeacherValidationException =
              new TeacherValidationException(InvalidTeacherException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(studentId))
                    .ReturnsAsync(storageTeacher);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(invalidTeacher.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(randomDate);
            randomTeacher.CreatedDate = randomTeacher.CreatedDate.AddMinutes(minutesInThePast);
            Teacher invalidTeacher = randomTeacher;
            invalidTeacher.UpdatedDate = randomDate;
            Teacher storageTeacher = randomTeacher.DeepClone();
            Guid studentId = invalidTeacher.Id;

            var InvalidTeacherException = new InvalidTeacherException(
                parameterName: nameof(Teacher.UpdatedDate),
                parameterValue: invalidTeacher.UpdatedDate);

            var expectedTeacherValidationException =
              new TeacherValidationException(InvalidTeacherException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(studentId))
                    .ReturnsAsync(storageTeacher);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Teacher> modifyTeacherTask =
                this.teacherService.ModifyTeacherAsync(invalidTeacher);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() =>
                modifyTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(invalidTeacher.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
