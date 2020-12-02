// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public void ShouldLogWarningOnRetrieveAllWhenCalendarEntriesWereEmptyAndLogIt()
        {
            // given
            IQueryable<CalendarEntry> emptyStorageCalendarEntries = new List<CalendarEntry>().AsQueryable();
            IQueryable<CalendarEntry> expectedCalendarEntries = emptyStorageCalendarEntries;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntries())
                    .Returns(expectedCalendarEntries);

            // when
            IQueryable<CalendarEntry> actualCalendarEntries =
                this.calendarEntryService.RetrieveAllCalendarEntries();

            // then
            actualCalendarEntries.Should().BeEquivalentTo(emptyStorageCalendarEntries);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntries(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No calendar entries found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
