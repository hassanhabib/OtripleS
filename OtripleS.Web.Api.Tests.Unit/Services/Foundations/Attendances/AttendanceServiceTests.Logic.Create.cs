﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Attendances;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attendances
{
    public partial class AttendanceServiceTests
    {
        [Fact]
        public async Task ShouldCreateAttendanceAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Attendance randomAttendance = CreateRandomAttendance(randomDateTime);
            randomAttendance.UpdatedBy = randomAttendance.CreatedBy;
            randomAttendance.UpdatedDate = randomAttendance.CreatedDate;
            Attendance inputAttendance = randomAttendance;
            Attendance storageAttendance = inputAttendance;
            Attendance expectedAttendance = storageAttendance;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttendanceAsync(inputAttendance))
                    .ReturnsAsync(storageAttendance);

            // when
            Attendance actualAttendance =
                await this.attendanceService.CreateAttendanceAsync(inputAttendance);

            // then
            actualAttendance.Should().BeEquivalentTo(expectedAttendance);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.AtLeastOnce);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(inputAttendance),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
