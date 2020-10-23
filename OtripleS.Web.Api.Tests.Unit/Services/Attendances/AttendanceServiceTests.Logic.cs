// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Attendances;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Attendances
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
                await this.attendanceService.DeleteAttendanceAsync(inputAttendanceId);

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
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
