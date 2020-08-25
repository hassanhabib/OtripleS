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
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomAttendanceId = Guid.NewGuid();
            Guid inputAttendanceId = randomAttendanceId;
            var sqlException = GetSqlException();

            var expectedDependencyException = 
                new AttendanceDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
              broker.SelectAttendanceByIdAsync(inputAttendanceId))
                  .ThrowsAsync(sqlException);

            // when
            ValueTask<Attendance> retrieveAttendanceTask =
                this.attendanceService.RetrieveAttendanceByIdAsync(inputAttendanceId);

            // then
            await Assert.ThrowsAsync<AttendanceDependencyException>(() => 
                retrieveAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
