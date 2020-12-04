// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntries
{
    public partial class CalendarEntryServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidatonExceptionOnRemoveWhenIdIsInvalidAndLogItAsync()
		{
			// given
			Guid invalidCalendarEntryId = Guid.Empty;

			var invalidCalendarEntryException = new InvalidCalendarEntryException(
				parameterName: nameof(CalendarEntry.Id),
				parameterValue: invalidCalendarEntryId);

			var expectedCalendarEntryValidationException =
				new CalendarEntryValidationException(invalidCalendarEntryException);

			// when
			ValueTask<CalendarEntry> deleteCalendarEntryTask =
				this.calendarEntryService.DeleteCalendarEntryByIdAsync(invalidCalendarEntryId);

			// then
			await Assert.ThrowsAsync<CalendarEntryValidationException>(() => deleteCalendarEntryTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
					Times.Once);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarEntryByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteCalendarEntryAsync(It.IsAny<CalendarEntry>()),
					Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageCalendarEntryIsInvalidAndLogItAsync()
		{
			// given
			Guid randomCalendarEntryId = Guid.NewGuid();
			Guid inputCalendarEntryId = randomCalendarEntryId;
			CalendarEntry invalidStorageCalendarEntry = null;

			var notFoundCalendarEntryException = 
				new NotFoundCalendarEntryException(inputCalendarEntryId);

			var expectedCalendarEntryValidationException =
				new CalendarEntryValidationException(notFoundCalendarEntryException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId))
					.ReturnsAsync(invalidStorageCalendarEntry);

			// when
			ValueTask<CalendarEntry> deleteCalendarEntryByIdTask =
				this.calendarEntryService.DeleteCalendarEntryByIdAsync(inputCalendarEntryId);

			// then
			await Assert.ThrowsAsync<CalendarEntryValidationException>(() => 
				deleteCalendarEntryByIdTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
					Times.Once);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteCalendarEntryAsync(It.IsAny<CalendarEntry>()),
					Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
