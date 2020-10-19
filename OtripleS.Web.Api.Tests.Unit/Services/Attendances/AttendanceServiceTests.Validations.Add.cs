// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Models.Attendances.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Attendances
{
    public partial class AttendanceServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAttendanceIsNullAndLogItAsync()
        {
            // given
            Attendance randomAttendance = null;
            Attendance nullAttendance = randomAttendance;
            var nullAttendanceException = new NullAttendanceException();

            var expectedAttendanceValidationException =
                new AttendanceValidationException(nullAttendanceException);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(nullAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
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
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTime);
            Attendance inputAttendance = randomAttendance;
            inputAttendance.Id = default;

            var invalidAttendanceException = new InvalidAttendanceException(
                parameterName: nameof(Attendance.Id),
                parameterValue: inputAttendance.Id);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            // when
            ValueTask<Attendance> registerAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(inputAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                registerAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnCreateWhenAttendanceSemesterCourseIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTime);
            Attendance invalidAttendance = randomAttendance;
            invalidAttendance.StudentSemesterCourseId = default;

            var invalidAttendanceException = new InvalidAttendanceException(
               parameterName: nameof(Attendance.StudentSemesterCourseId),
               parameterValue: invalidAttendance.StudentSemesterCourseId);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(invalidAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async Task ShouldThrowValidationExceptionOnCreateWhenAttendanceDateIsInvalidAndLogItAsync(
            int invallidMinutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTime);
            Attendance invalidAttendance = randomAttendance;
            invalidAttendance.UpdatedBy = invalidAttendance.CreatedBy;
            invalidAttendance.UpdatedDate = invalidAttendance.CreatedDate;
            invalidAttendance.AttendanceDate = dateTime.AddMinutes(invallidMinutes);

            var invalidAttendanceException = new InvalidAttendanceException(
               parameterName: nameof(Attendance.AttendanceDate),
               parameterValue: invalidAttendance.AttendanceDate);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(invalidAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTime);
            Attendance inputAttendance = randomAttendance;
            inputAttendance.CreatedBy = default;

            var invalidAttendanceException = new InvalidAttendanceException(
                parameterName: nameof(Attendance.CreatedBy),
                parameterValue: inputAttendance.CreatedBy);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(inputAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTime);
            Attendance inputAttendance = randomAttendance;
            inputAttendance.CreatedDate = default;

            var invalidAttendanceException = new InvalidAttendanceException(
                parameterName: nameof(Attendance.CreatedDate),
                parameterValue: inputAttendance.CreatedDate);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(inputAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int invallidMinutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTime);
            Attendance inputAttendance = randomAttendance;
            inputAttendance.UpdatedBy = inputAttendance.CreatedBy;
            inputAttendance.CreatedDate = dateTime.AddMinutes(invallidMinutes);
            inputAttendance.UpdatedDate = inputAttendance.CreatedDate;

            var invalidAttendanceException = new InvalidAttendanceException(
                parameterName: nameof(Attendance.CreatedDate),
                parameterValue: inputAttendance.CreatedDate);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(inputAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.AtLeastOnce);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTime);
            Attendance inputAttendance = randomAttendance;
            inputAttendance.UpdatedBy = default;

            var invalidAttendanceException = new InvalidAttendanceException(
                parameterName: nameof(Attendance.UpdatedBy),
                parameterValue: inputAttendance.UpdatedBy);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(inputAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTime);
            Attendance inputAttendance = randomAttendance;
            inputAttendance.UpdatedDate = default;

            var invalidAttendanceException = new InvalidAttendanceException(
                parameterName: nameof(Attendance.UpdatedDate),
                parameterValue: inputAttendance.UpdatedDate);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(inputAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime);
            Attendance inputAttendance = randomAttendance.DeepClone();
            inputAttendance.UpdatedBy = randomAttendance.CreatedBy;
            inputAttendance.UpdatedDate = GetRandomDateTime();

            var invalidAttendanceException = new InvalidAttendanceException(
                parameterName: nameof(Attendance.UpdatedDate),
                parameterValue: inputAttendance.UpdatedDate);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(inputAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAttendanceAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime);
            Attendance alreadyExistsAttendance = randomAttendance;
            alreadyExistsAttendance.UpdatedBy = alreadyExistsAttendance.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsAttendanceException =
                new AlreadyExistsAttendanceException(duplicateKeyException);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(alreadyExistsAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttendanceAsync(alreadyExistsAttendance))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(alreadyExistsAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.AtLeastOnce);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(alreadyExistsAttendance),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
