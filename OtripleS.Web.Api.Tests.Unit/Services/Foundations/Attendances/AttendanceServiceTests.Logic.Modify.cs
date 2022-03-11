// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ShouldModifyAttendanceAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(randomInputDate);
            Attendance inputAttendance = randomAttendance;
            Attendance afterUpdateStorageAttendance = inputAttendance;
            Attendance expectedAttendance = afterUpdateStorageAttendance;
            Attendance beforeUpdateStorageAttendance = randomAttendance.DeepClone();
            inputAttendance.UpdatedDate = randomDate;
            Guid attendanceId = inputAttendance.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(attendanceId))
                    .ReturnsAsync(beforeUpdateStorageAttendance);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAttendanceAsync(inputAttendance))
                    .ReturnsAsync(afterUpdateStorageAttendance);

            // when
            Attendance actualAttendance =
                await this.attendanceService.ModifyAttendanceAsync(inputAttendance);

            // then
            actualAttendance.Should().BeEquivalentTo(expectedAttendance);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(attendanceId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAttendanceAsync(inputAttendance),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
