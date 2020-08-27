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
    }
}
