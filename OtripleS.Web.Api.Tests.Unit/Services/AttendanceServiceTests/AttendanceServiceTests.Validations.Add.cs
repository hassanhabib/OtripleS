// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Models.Attendances.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.AttendanceServiceTests
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
                broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()),
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

            var invalidAttendanceInputException = new InvalidAttendanceException(
                parameterName: nameof(Attendance.Id),
                parameterValue: inputAttendance.Id);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceInputException);

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
                broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnCreateWhenStudentSemesterCourseIdIsInvalidAndLogItAsync()
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
                broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
