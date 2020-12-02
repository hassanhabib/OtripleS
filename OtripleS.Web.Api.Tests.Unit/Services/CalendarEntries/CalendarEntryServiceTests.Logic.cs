// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntries;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntries
{
    public partial class CalendarEntryServiceTests
	{
		[Fact]
		public void ShouldRetrieveAllCalendarEntries()
        {
			// given
			IQueryable<CalendarEntry> randomCalendarEntries = CreateRandomCalendarEntries();
			IQueryable<CalendarEntry> storageCalendarEntries = randomCalendarEntries;
			IQueryable<CalendarEntry> expectedCalendarEntries = storageCalendarEntries;

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllCalendarEntries())
					.Returns(storageCalendarEntries);

			// when
			IQueryable<CalendarEntry> actualCalendarEntries =
				this.calendarEntryService.RetrieveAllCalendarEntries();

			// then
			actualCalendarEntries.Should().BeEquivalentTo(expectedCalendarEntries);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllCalendarEntries(),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
