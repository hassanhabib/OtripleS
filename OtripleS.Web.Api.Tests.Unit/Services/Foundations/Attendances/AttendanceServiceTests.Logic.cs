// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Attendances;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attendances
{
    public partial class AttendanceServiceTests
    {       
        [Fact]
        public async Task ShouldRetrieveAttendanceByIdAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = GetRandomDateTime();
            Guid randomAttendanceId = Guid.NewGuid();
            Guid inputAttendanceId = randomAttendanceId;
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTimeOffset);
            randomAttendance.Id = randomAttendanceId;
            Attendance storageAttendance = randomAttendance;
            Attendance expectedAttendance = randomAttendance;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId))
                    .ReturnsAsync(storageAttendance);

            // when
            Attendance actualAttendance =
                await this.attendanceService.RetrieveAttendanceByIdAsync(inputAttendanceId);

            // then
            actualAttendance.Should().BeEquivalentTo(expectedAttendance);

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

        [Fact]
        public async Task ShouldDeleteAttendanceAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTime);
            Guid inputAttendanceId = randomAttendance.Id;
            Attendance inputAttendance = randomAttendance;
            Attendance storageAttendance = inputAttendance;
            Attendance expectedAttendance = storageAttendance;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId))
                    .ReturnsAsync(inputAttendance);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAttendanceAsync(inputAttendance))
                    .ReturnsAsync(storageAttendance);

            // when
            Attendance actualAttendance =
                await this.attendanceService.RemoveAttendanceByIdAsync(inputAttendanceId);

            // then
            actualAttendance.Should().BeEquivalentTo(expectedAttendance);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttendanceAsync(inputAttendance),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }        
    }
}
