// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Models.Attendances.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attendances
{
    public partial class AttendanceServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttendanceId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var expectedDependencyException =
                new AttendanceDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(someAttendanceId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Attendance> retrieveAttendanceTask =
                this.attendanceService.RetrieveAttendanceByIdAsync(someAttendanceId);

            // then
            await Assert.ThrowsAsync<AttendanceDependencyException>(() =>
                retrieveAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(someAttendanceId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttendanceId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedAttendanceDependencyException =
                new AttendanceDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(someAttendanceId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Attendance> retrieveAttendanceTask =
                this.attendanceService.RetrieveAttendanceByIdAsync(someAttendanceId);

            // then
            await Assert.ThrowsAsync<AttendanceDependencyException>(() =>
                retrieveAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(someAttendanceId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given 
            Guid someAttendanceId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAttendanceServiceException =
                new FailedAttendanceServiceException(serviceException);

            var expectedAttendanceServiceException =
                new AttendanceServiceException(failedAttendanceServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(someAttendanceId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Attendance> retrieveAttendanceByIdTask =
                this.attendanceService.RetrieveAttendanceByIdAsync(someAttendanceId);

            // then
            await Assert.ThrowsAsync<AttendanceServiceException>(() =>
                retrieveAttendanceByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(someAttendanceId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
