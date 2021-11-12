// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Services.Foundations.Attendances;
using Tynamix.ObjectFiller;
using Xeptions;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attendances
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

        private static Attendance CreateRandomAttendance(DateTimeOffset dateTime) =>
            GetAttendanceFiller(dateTime).Create();

        private static IQueryable<Attendance> CreateRandomAttendances(DateTimeOffset dates) =>
            GetAttendanceFiller(dates).Create(GetRandomNumber()).AsQueryable();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message &&
                actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();

        public static IEnumerable<object[]> InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new List<object[]>
            {
                new object[] { randomMoreThanMinuteFromNow },
                new object[] { randomMoreThanMinuteBeforeNow }
            };
        }

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static Filler<Attendance> GetAttendanceFiller(DateTimeOffset dateTime)
        {
            var attendance = new Filler<Attendance>();

            attendance.Setup()
                .OnProperty(attendance => attendance.Status).Use(AttendanceStatus.Present)
                .OnType<DateTimeOffset>().Use(dateTime);

            return attendance;
        }        
    }
}