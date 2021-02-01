// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Services.CalendarEntryAttachments;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICalendarEntryAttachmentService calendarEntryAttachmentService;

        public CalendarEntryAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.calendarEntryAttachmentService = new CalendarEntryAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private CalendarEntryAttachment CreateRandomCalendarEntryAttachment() =>
          CreateCalendarEntryAttachmentFiller(DateTimeOffset.UtcNow).Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 100).GetValue();
        private static Filler<CalendarEntryAttachment> CreateCalendarEntryAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<CalendarEntryAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(calendarEntryAttachment => calendarEntryAttachment.CalendarEntry).IgnoreIt()
                .OnProperty(calendarEntryAttachment => calendarEntryAttachment.Attachment).IgnoreIt();

            return filler;
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }

        private static SqlException GetSqlException() =>
          (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static IQueryable<CalendarEntryAttachment> CreateRandomCalendarEntries(DateTimeOffset dateTime) =>
            CreateCalendarEntryAttachmentFiller(dateTime).Create(GetRandomNumber()).AsQueryable();
    }
}
