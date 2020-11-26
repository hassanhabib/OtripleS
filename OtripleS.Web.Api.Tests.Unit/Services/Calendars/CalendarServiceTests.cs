// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Services.Calendars;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.Calendars
{
	public partial class CalendarServiceTests
	{
		private readonly Mock<IStorageBroker> storageBrokerMock;
		private readonly ICalendarService calendarService;
		private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;

		public CalendarServiceTests()
		{
			this.storageBrokerMock = new Mock<IStorageBroker>();
			this.loggingBrokerMock = new Mock<ILoggingBroker>();
			this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

			this.calendarService = new CalendarService(
				storageBroker: this.storageBrokerMock.Object,
				loggingBroker: this.loggingBrokerMock.Object,
				dateTimeBroker: this.dateTimeBrokerMock.Object);
		}

		private Calendar CreateRandomCalendar(DateTimeOffset dates) =>
			CreateCalendarFiller(dates).Create();

		private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
		private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
		private static DateTimeOffset GetRandomDateTime() =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();

		private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
		{
			return actualException =>
				expectedException.Message == actualException.Message
				&& expectedException.InnerException.Message == actualException.InnerException.Message;
		}

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

		private static SqlException GetSqlException() =>
			(SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

		private static Filler<Calendar> CreateCalendarFiller(DateTimeOffset dates)
		{
			var filler = new Filler<Calendar>();

			filler.Setup()
				.OnProperty(Calendar => Calendar.CreatedDate).Use(dates)
				.OnProperty(Calendar => Calendar.UpdatedDate).Use(dates);

			return filler;
		}
	}
}
