// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Services.CalendarEntries;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntries
{
    public partial class CalendarEntryServiceTests
	{
		private readonly Mock<IStorageBroker> storageBrokerMock;
		private readonly ICalendarEntryService calendarEntryService;
		private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;

		public CalendarEntryServiceTests()
		{
			this.storageBrokerMock = new Mock<IStorageBroker>();
			this.loggingBrokerMock = new Mock<ILoggingBroker>();
			this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

			this.calendarEntryService = new CalendarEntryService(
				storageBroker: this.storageBrokerMock.Object,
				loggingBroker: this.loggingBrokerMock.Object,
				dateTimeBroker: this.dateTimeBrokerMock.Object);
		}

		private IQueryable<CalendarEntry> CreateRandomCalendarEntries() =>
			CreateCalendarEntryFiller(dates: DateTimeOffset.UtcNow)
				.Create(GetRandomNumber()).AsQueryable();

		private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

		private static Filler<CalendarEntry> CreateCalendarEntryFiller(DateTimeOffset dates)
		{
			var filler = new Filler<CalendarEntry>();

			filler.Setup()
				.OnType<DateTimeOffset>().Use(dates)
				.OnType<DateTimeOffset?>().IgnoreIt()
				.OnProperty(calendarEntry => calendarEntry.Calendar).IgnoreIt();

			return filler;
		}
	}
}
