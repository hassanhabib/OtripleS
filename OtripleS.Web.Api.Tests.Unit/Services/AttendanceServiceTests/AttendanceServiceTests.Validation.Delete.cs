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
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttendanceId = default;
            Guid inputAttendanceId = randomAttendanceId;

            var invalidAttendanceInputException = new InvalidAttendanceInputException(
                parameterName: nameof(Attendance.Id),
                parameterValue: inputAttendanceId);

            var expectedAttendanceValidationException = new AttendanceValidationException(invalidAttendanceInputException);

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
    }
}
