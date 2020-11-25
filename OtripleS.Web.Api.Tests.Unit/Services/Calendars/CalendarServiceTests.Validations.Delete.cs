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
		public async Task ShouldThrowValidatonExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
		{
			// given
			Guid randomCalendarId = default;
			Guid inputCalendarId = randomCalendarId;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.Id),
				parameterValue: inputCalendarId);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> actualCalendarTask =
				this.calendarService.DeleteCalendarByIdAsync(inputCalendarId);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() => actualCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteCalendarAsync(It.IsAny<Calendar>()),
					Times.Never);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageCalendarIsInvalidAndLogItAsync()
		{
			// given
			DateTimeOffset dateTime = GetRandomDateTime();
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Guid inputCalendarId = randomCalendar.Id;
			Calendar inputCalendar = randomCalendar;
			Calendar nullStorageCalendar = null;

			var notFoundCalendarException = new NotFoundCalendarException(inputCalendarId);

			var expectedCalendarValidationException =
				new CalendarValidationException(notFoundCalendarException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectCalendarByIdAsync(inputCalendarId))
					.ReturnsAsync(nullStorageCalendar);

			// when
			ValueTask<Calendar> actualCalendarTask =
				this.calendarService.DeleteCalendarByIdAsync(inputCalendarId);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() => actualCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(inputCalendarId),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteCalendarAsync(It.IsAny<Calendar>()),
					Times.Never);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
