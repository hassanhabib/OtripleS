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

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public async Task ShouldThrowValidationExceptionOnModifyWhenCalendarLabelIsInvalidAndLogItAsync(
				  string invalidCalendarLabel)
		{
			// given
			Calendar randomCalendar = CreateRandomCalendar(DateTime.Now);
			Calendar invalidCalendar = randomCalendar;
			invalidCalendar.Label = invalidCalendarLabel;

			var invalidCalendarInputException = new InvalidCalendarInputException(
			   parameterName: nameof(Calendar.Label),
			   parameterValue: invalidCalendar.Label);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(invalidCalendar);

			// then
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
		public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
		{
			// given
			DateTimeOffset dateTime = GetRandomDateTime();
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar inputCalendar = randomCalendar;
			inputCalendar.CreatedBy = default;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.CreatedBy),
				parameterValue: inputCalendar.CreatedBy);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(inputCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
		{
			// given
			DateTimeOffset dateTime = GetRandomDateTime();
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar inputCalendar = randomCalendar;
			inputCalendar.UpdatedBy = default;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.UpdatedBy),
				parameterValue: inputCalendar.UpdatedBy);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(inputCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
		{
			// given
			DateTimeOffset dateTime = GetRandomDateTime();
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar inputCalendar = randomCalendar;
			inputCalendar.CreatedDate = default;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.CreatedDate),
				parameterValue: inputCalendar.CreatedDate);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(inputCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
