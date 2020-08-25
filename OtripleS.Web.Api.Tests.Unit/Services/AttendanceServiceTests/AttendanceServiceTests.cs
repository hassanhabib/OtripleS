// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Services.Attendances;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.AttendanceServiceTests
{
    public partial class AttendanceServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IAttendanceService attendanceService;

        public AttendanceServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.attendanceService = new AttendanceService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static Attendance CreateRandomAttedance(DateTimeOffset dateTime) =>
            GetAttendanceFiller(dateTime).Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<Attendance> GetAttendanceFiller(DateTimeOffset dateTime)
        {
            Filler<Attendance> attendance = new Filler<Attendance>();

            attendance.Setup()
                .OnProperty(attendance => attendance.StudentSemesterCourseId).IgnoreIt()
                .OnType<DateTimeOffset>().Use(dateTime);

            return attendance;
        }

    }
}
