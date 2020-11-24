// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Calendars
{
	public partial class CalendarServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyWhenCalendarIsNullAndLogItAsync()
		{
			//given
			Calendar invalidCalendar = null;
			var nullCalendarException = new NullCalendarException();

			var expectedCalendarValidationException =
				new CalendarValidationException(nullCalendarException);

			//when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(invalidCalendar);

			//then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
				Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyWhenCalendarIdIsInvalidAndLogItAsync()
		{
			//given
			Guid invalidCalendarId = Guid.Empty;
			DateTimeOffset dateTime = GetRandomDateTime();
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar invalidCalendar = randomCalendar;
			invalidCalendar.Id = invalidCalendarId;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.Id),
				parameterValue: invalidCalendar.Id);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			//when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(invalidCalendar);

			//then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
				Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
