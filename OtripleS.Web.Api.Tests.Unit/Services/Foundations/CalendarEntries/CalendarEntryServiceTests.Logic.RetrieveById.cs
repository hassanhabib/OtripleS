// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntries;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntries
{
    public partial class CalendarEntryServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveCalendarEntryByIdAsync()
        {
            // given
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid inputCalendarEntryId = randomCalendarEntryId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(randomDateTime);
            CalendarEntry storageCalendarEntry = randomCalendarEntry;
            CalendarEntry expectedCalendarEntry = storageCalendarEntry;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId))
                    .ReturnsAsync(storageCalendarEntry);

            // when
            CalendarEntry actualCalendarEntry =
                await this.calendarEntryService.RetrieveCalendarEntryByIdAsync(
                    inputCalendarEntryId);

            // then
            actualCalendarEntry.Should().BeEquivalentTo(expectedCalendarEntry);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
