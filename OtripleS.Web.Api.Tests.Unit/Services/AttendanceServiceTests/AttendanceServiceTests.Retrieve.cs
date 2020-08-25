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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsNullAndLogItAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = GetRandomDateTime();
            Guid randomAttendanceId = default;
            Guid inputAttendanceId = randomAttendanceId;

            var invalidAttendanceInputException = new InvalidAttendanceInputException(
                parameterName: nameof(Attendance.Id),
                parameterValue: inputAttendanceId);

            var expectedValidationException = 
                new AttendanceValidationException(invalidAttendanceInputException);

            // when
            ValueTask<Attendance> retrieveAttendanceTask =
                this.attendanceService.RetrieveAttendanceByIdAsync(inputAttendanceId);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() => 
                retrieveAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
