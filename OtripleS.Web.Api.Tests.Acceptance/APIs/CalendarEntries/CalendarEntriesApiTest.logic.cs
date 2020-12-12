// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.CalendarEntries;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.CalendarEntries
{
    public partial class CalendarEntriesApiTest
    {
        [Fact]
        public async Task ShouldPostCalendarEntryAsync()
        {
            // given
            CalendarEntry randomCalendarEntry = await CreateRandomCalendarEntry();
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            CalendarEntry expectedCalendarEntry = inputCalendarEntry;

            // when 
            await this.otripleSApiBroker.PostCalendarEntryAsync(inputCalendarEntry);

            CalendarEntry actualCalendarEntry =
                 await this.otripleSApiBroker.GetCalendarEntryByIdAsync(inputCalendarEntry.Id);

            // then
            actualCalendarEntry.Should().BeEquivalentTo(expectedCalendarEntry);
            await DeleteCalendarEntryAsync(actualCalendarEntry);
        }

        [Fact]
        public async Task ShouldPutCalendarEntryAsync()
        {
            // given
            CalendarEntry randomCalendarEntry = await PostRandomCalendarEntryAsync();
            CalendarEntry modifiedCalendarEntry = await UpdateCalendarEntryRandom(randomCalendarEntry);

            // when
            await this.otripleSApiBroker.PutCalendarEntryAsync(modifiedCalendarEntry);

            CalendarEntry actualCalendarEntry =
                await this.otripleSApiBroker.GetCalendarEntryByIdAsync(randomCalendarEntry.Id);

            // then
            actualCalendarEntry.Should().BeEquivalentTo(modifiedCalendarEntry);
            await DeleteCalendarEntryAsync(actualCalendarEntry);
        }

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
            List<CalendarEntry> actualcalendarEntries =
                await this.otripleSApiBroker.GetAllCalendarEntriesAsync();

            //then
            foreach (CalendarEntry expectedCalendarEntry in expectedCalendarEntries)
            {
                CalendarEntry actualCalendarEntry =
                    actualcalendarEntries.Single(calendarEntry =>
                        calendarEntry.Id == expectedCalendarEntry.Id);

                actualCalendarEntry.Should().BeEquivalentTo(expectedCalendarEntry);
                await DeleteCalendarEntryAsync(actualCalendarEntry);
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
                await DeleteCalendarEntryAsync(inputCalendarEntry);

            ValueTask<CalendarEntry> getCalendarEntryByIdTask =
                this.otripleSApiBroker.GetCalendarEntryByIdAsync(inputCalendarEntry.Id);

            // then
            deletedCalendarEntry.Should().BeEquivalentTo(expectedCalendarEntry);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCalendarEntryByIdTask.AsTask());
        }
    }
}
