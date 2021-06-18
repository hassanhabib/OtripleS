// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.CalendarEntries;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Calendars;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.CalendarEntries
{
    [Collection(nameof(ApiTestCollection))]
    public partial class CalendarEntriesApiTest
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public CalendarEntriesApiTest(OtripleSApiBroker calendarEntryApiBroker) =>
            this.otripleSApiBroker = calendarEntryApiBroker;

        private async ValueTask<CalendarEntry> PostRandomCalendarEntryAsync()
        {
            CalendarEntry randomCalendarEntry = await CreateRandomCalendarEntry();
            await this.otripleSApiBroker.PostCalendarEntryAsync(randomCalendarEntry);

            return randomCalendarEntry;
        }

        private async ValueTask<CalendarEntry> DeleteCalendarEntryAsync(CalendarEntry actualCalendarEntry)
        {
            CalendarEntry deletedCalendarEntry =
                await otripleSApiBroker.DeleteCalendarEntryByIdAsync(actualCalendarEntry.Id);

            await otripleSApiBroker.DeleteCalendarByIdAsync(actualCalendarEntry.CalendarId);

            return deletedCalendarEntry;
        }

        private async ValueTask<Calendar> PostRandomGuardianAsync()
        {
            Calendar randomCalendar = CreateRandomCalendar();
            await this.otripleSApiBroker.PostCalendarAsync(randomCalendar);

            return randomCalendar;
        }

        private static string GetRandomString() => new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<CalendarEntry> UpdateCalendarEntryRandom(CalendarEntry calendarEntry)
        {
            Calendar calendar = await PostRandomGuardianAsync();
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<CalendarEntry>();

            filler.Setup()
                .OnProperty(calendarEntry => calendarEntry.Id).Use(calendarEntry.Id)
                .OnProperty(calendarEntry => calendarEntry.CreatedBy).Use(calendarEntry.CreatedBy)
                .OnProperty(calendarEntry => calendarEntry.UpdatedBy).Use(calendarEntry.UpdatedBy)
                .OnProperty(calendarEntry => calendarEntry.CreatedDate).Use(calendarEntry.CreatedDate)
                .OnProperty(calendarEntry => calendarEntry.UpdatedDate).Use(now)
                .OnProperty(calendarEntry => calendarEntry.CalendarId).Use(calendar.Id)
                .OnProperty(calenderEntry => calenderEntry.RepeatUntil).IgnoreIt()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private async ValueTask<CalendarEntry> CreateRandomCalendarEntry()
        {
            Calendar calendar = await PostRandomGuardianAsync();
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<CalendarEntry>();

            filler.Setup()
                .OnProperty(calendarEntry => calendarEntry.Label).Use(GetRandomString())
                .OnProperty(calendarEntry => calendarEntry.CreatedBy).Use(calendar.CreatedBy)
                .OnProperty(calendarEntry => calendarEntry.UpdatedBy).Use(calendar.CreatedBy)
                .OnProperty(calendarEntry => calendarEntry.CreatedDate).Use(now)
                .OnProperty(calendarEntry => calendarEntry.UpdatedDate).Use(now)
                .OnProperty(calendarEntry => calendarEntry.CalendarId).Use(calendar.Id)
                .OnProperty(calenderEntry => calenderEntry.RepeatUntil).IgnoreIt()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static Calendar CreateRandomCalendar()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Calendar>();

            filler.Setup()
                .OnProperty(calendarEntry => calendarEntry.CreatedBy).Use(posterId)
                .OnProperty(calendarEntry => calendarEntry.UpdatedBy).Use(posterId)
                .OnProperty(calendarEntry => calendarEntry.CreatedDate).Use(now)
                .OnProperty(calendarEntry => calendarEntry.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }
    }
}
