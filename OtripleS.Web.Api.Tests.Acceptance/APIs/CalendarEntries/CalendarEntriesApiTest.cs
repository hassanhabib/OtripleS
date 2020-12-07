// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
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
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry();
            await this.otripleSApiBroker.PostCalendarEntryAsync(randomCalendarEntry);

            return randomCalendarEntry;
        }

        private IEnumerable<CalendarEntry> GetRandomCalendarEntries() =>
            this.CreateRandomCalendarEntryFiller().Create(GetRandomNumber());
        
        private CalendarEntry CreateRandomCalendarEntry() =>
             CreateRandomCalendarEntryFiller().Create();

        private CalendarEntry UpdateCalendarEntryRandom(CalendarEntry calendarEntry)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<CalendarEntry>();

            filler.Setup()
                .OnProperty(calendarEntry => calendarEntry.Id).Use(calendarEntry.Id)
                .OnProperty(calendarEntry => calendarEntry.CreatedBy).Use(calendarEntry.CreatedBy)
                .OnProperty(calendarEntry => calendarEntry.UpdatedBy).Use(calendarEntry.UpdatedBy)
                .OnProperty(calendarEntry => calendarEntry.CreatedDate).Use(calendarEntry.CreatedDate)
                .OnProperty(calendarEntry => calendarEntry.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private Filler<CalendarEntry> CreateRandomCalendarEntryFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<CalendarEntry>();

            filler.Setup()
                .OnProperty(classroom => classroom.CreatedBy).Use(posterId)
                .OnProperty(classroom => classroom.UpdatedBy).Use(posterId)
                .OnProperty(classroom => classroom.CreatedDate).Use(now)
                .OnProperty(classroom => classroom.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
    }
}
