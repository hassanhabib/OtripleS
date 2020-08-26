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
        public async Task ShouldThrowValidationExceptionOnModifyWhenAttendanceIsNullAndLogItAsync()
        {
            //given
            Attendance invalidAttendance = null;
            var nullAttendanceException = new NullAttendanceException();

            var expectedAttendanceValidationException =
                new AttendanceValidationException(nullAttendanceException);

            //when
            ValueTask<Attendance> modifyAttendanceTask =
                this.attendanceService.ModifyAttendanceAsync(invalidAttendance);

            //then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                modifyAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenAttendanceIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidAttendanceId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime);
            Attendance invalidAttendance = randomAttendance;
            invalidAttendance.Id = invalidAttendanceId;

            var invalidAttendanceException = new InvalidAttendanceInputException(
                parameterName: nameof(Attendance.Id),
                parameterValue: invalidAttendance.Id);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            //when
            ValueTask<Attendance> modifyAttendanceTask =
                this.attendanceService.ModifyAttendanceAsync(invalidAttendance);

            //then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                modifyAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
