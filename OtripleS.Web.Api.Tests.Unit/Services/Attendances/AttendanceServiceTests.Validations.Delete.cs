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

namespace OtripleS.Web.Api.Tests.Unit.Services.Attendances
{
    public partial class AttendanceServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttendanceId = default;
            Guid inputAttendanceId = randomAttendanceId;

            var invalidAttendanceException = new InvalidAttendanceException(
                parameterName: nameof(Attendance.Id),
                parameterValue: inputAttendanceId);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            // when
            ValueTask<Attendance> actualAttendanceTask =
                this.attendanceService.DeleteAttendanceAsync(inputAttendanceId);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() => actualAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageAttendanceIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTimeOffset);
            Guid inputAttendanceId = randomAttendance.Id;
            Attendance inputAttendance = randomAttendance;
            Attendance nullStorageAttendance = null;

            var notFoundAttendanceException = new NotFoundAttendanceException(inputAttendanceId);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(notFoundAttendanceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId))
                    .ReturnsAsync(nullStorageAttendance);

            // when
            ValueTask<Attendance> actualAttendanceTask =
                this.attendanceService.DeleteAttendanceAsync(inputAttendanceId);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() => actualAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
