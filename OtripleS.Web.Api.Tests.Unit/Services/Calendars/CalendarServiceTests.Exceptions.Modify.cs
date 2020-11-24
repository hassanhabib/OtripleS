// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Calendars
{
	public partial class CalendarServiceTests
	{
		[Fact]
		public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
		{
			// given
			int randomNegativeNumber = GetNegativeRandomNumber();
			DateTimeOffset randomDateTime = GetRandomDateTime();
			Calendar randomCalendar = CreateRandomCalendar(randomDateTime);
			Calendar someCalendar = randomCalendar;
			someCalendar.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
			SqlException sqlException = GetSqlException();

			var expectedCalendarDependencyException =
				new CalendarDependencyException(sqlException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectCalendarByIdAsync(someCalendar.Id))
					.ThrowsAsync(sqlException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTime())
					.Returns(randomDateTime);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(someCalendar);

			// then
			await Assert.ThrowsAsync<CalendarDependencyException>(() =>
				modifyCalendarTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(someCalendar.Id),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(expectedCalendarDependencyException))),
					Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
