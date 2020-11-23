// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attendances;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Attendances
{
    [Collection(nameof(ApiTestCollection))]
    public partial class AttendancesApiTests
    {
        private OtripleSApiBroker otripleSApiBroker;

        public AttendancesApiTests(OtripleSApiBroker otripleSApiBroker)
        {
            this.otripleSApiBroker = otripleSApiBroker;
        }
        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private IEnumerable<Attendance> GetRandomAttendances() =>
            CreateRandomAttendanceFiller().Create(GetRandomNumber());

        private Attendance CreateRandomAttendance() =>
            CreateRandomAttendanceFiller().Create();

        private Filler<Attendance> CreateRandomAttendanceFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Attendance>();

            filler.Setup()
                .OnProperty(attendance => attendance.CreatedBy).Use(posterId)
                .OnProperty(attendance => attendance.UpdatedBy).Use(posterId)
                .OnProperty(attendance => attendance.CreatedDate).Use(now)
                .OnProperty(attendance => attendance.UpdatedDate).Use(now)
                .OnProperty(Attendance => Attendance.AttendanceDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }


        private Attendance UpdateAttendanceRandom(Attendance inputAttendance)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Attendance>();

            filler.Setup()
                .OnProperty(attendance => attendance.Id).Use(inputAttendance.Id)
                .OnProperty(attendance => attendance.CreatedBy).Use(inputAttendance.CreatedBy)
                .OnProperty(attendance => attendance.UpdatedBy).Use(inputAttendance.UpdatedBy)
                .OnProperty(attendance => attendance.CreatedDate).Use(inputAttendance.CreatedDate)
                .OnProperty(attendance => attendance.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }


        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}