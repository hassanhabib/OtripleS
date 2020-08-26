// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
    }
}
