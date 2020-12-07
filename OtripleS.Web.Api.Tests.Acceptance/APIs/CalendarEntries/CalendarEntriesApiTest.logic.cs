// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.CalendarEntries;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.CalendarEntries
{
    public partial class CalendarEntriesApiTest
    {
        [Fact]
        public async Task ShouldGetAllCalendarEntriesAsync()
        {
            //given
            var randomCalendarEntries = new List<CalendarEntry>();

            for (var j = 0; j <= GetRandomNumber(); j++)
            {
                randomCalendarEntries.Add(await PostRandomCalendarEntryAsync());
            }

            List<CalendarEntry> inputedCalendarEntries = randomCalendarEntries;
            List<CalendarEntry> expectedCalendarEntries = inputedCalendarEntries;

            //When
            List<CalendarEntry> actualcalendarEntries = await this.otripleSApiBroker.GetAllCalendarEntriesAsync();

            //then
            foreach (CalendarEntry expectCalendarEntry in expectedCalendarEntries)
            {
                CalendarEntry actualCalendarEntry = actualcalendarEntries.Single(calendarEntry => calendarEntry.Id == expectCalendarEntry.Id);

                actualCalendarEntry.Should().BeEquivalentTo(expectCalendarEntry);
                await otripleSApiBroker.DeleteCalenderEntryByIdAsync(actualCalendarEntry.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteCalendarAsync()
        {
            //given
            CalendarEntry randomCalendarEntry = await PostRandomCalendarEntryAsync();
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            CalendarEntry expectedCalendarEntry = inputCalendarEntry;

            //when
            CalendarEntry deletedCalendarEntry =
                await this.otripleSApiBroker.DeleteCalenderEntryByIdAsync(inputCalendarEntry.Id);

            ValueTask<CalendarEntry> getCalendarEntryByIdTask =
                this.otripleSApiBroker.DeleteCalenderEntryByIdAsync(inputCalendarEntry.Id);

            // then
            deletedCalendarEntry.Should().BeEquivalentTo(expectedCalendarEntry);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCalendarEntryByIdTask.AsTask());
        }
    }
}
