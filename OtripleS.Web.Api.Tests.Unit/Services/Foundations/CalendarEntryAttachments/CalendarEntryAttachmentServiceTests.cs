// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Services.Foundations.CalendarEntryAttachments;
using Tynamix.ObjectFiller;
using Xeptions;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntryAttachments
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

        private static IQueryable<CalendarEntryAttachment> CreateRandomCalendarEntryAttachments() =>
            CreateCalendarEntryAttachmentFiller(DateTimeOffset.UtcNow).Create(GetRandomNumber()).AsQueryable();

        private static int GetRandomNumber() => 
            new IntRange(min: 2, max: 150).GetValue();

        private static string GetRandomMessage() => 
            new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static CalendarEntryAttachment CreateRandomCalendarEntryAttachment() =>
            CreateCalendarEntryAttachmentFiller(DateTimeOffset.UtcNow).Create();

        private static CalendarEntryAttachment CreateRandomCalendarEntryAttachment(DateTimeOffset dates) =>
            CreateCalendarEntryAttachmentFiller(dates).Create();

        private static SqlException GetSqlException() =>
          (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

        private static Filler<CalendarEntryAttachment> CreateCalendarEntryAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<CalendarEntryAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(calendarEntryAttachment => calendarEntryAttachment.CalendarEntry).IgnoreIt()
                .OnProperty(calendarEntryAttachment => calendarEntryAttachment.Attachment).IgnoreIt();

            return filler;
        }
    }
}
