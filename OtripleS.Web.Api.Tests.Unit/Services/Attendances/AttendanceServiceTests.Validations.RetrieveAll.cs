// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Attendances;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Attendances
{
    public partial class AttendanceServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenAttendancesWasEmptyAndLogIt()
        {
            // given
            IQueryable<Attendance> emptyStorageAttendances = new List<Attendance>().AsQueryable();
            IQueryable<Attendance> expectedAttendances = emptyStorageAttendances;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttendances())
                    .Returns(expectedAttendances);

            // when
            IQueryable<Attendance> actualAttendances =
                this.attendanceService.RetrieveAllAttendances();

            // then
            actualAttendances.Should().BeEquivalentTo(emptyStorageAttendances);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No Attendances found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttendances(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
