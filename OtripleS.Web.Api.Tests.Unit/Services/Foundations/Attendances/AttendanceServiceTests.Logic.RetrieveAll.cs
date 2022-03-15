// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Attendances;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attendances
{
    public partial class AttendanceServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllAttendances()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Attendance> randomAttendances = CreateRandomAttendances(randomDateTime);
            IQueryable<Attendance> storageAttendances = randomAttendances;
            IQueryable<Attendance> expectedAttendances = storageAttendances;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttendances())
                    .Returns(storageAttendances);

            // when
            IQueryable<Attendance> actualAttendances =
                this.attendanceService.RetrieveAllAttendances();

            // then
            actualAttendances.Should().BeEquivalentTo(expectedAttendances);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttendances(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
