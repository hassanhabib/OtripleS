// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Calendars;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Calendars
{
    [Collection(nameof(ApiTestCollection))]
    public partial class CalendarsApiTest
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public CalendarsApiTest(OtripleSApiBroker calendarApiBroker) =>
            this.otripleSApiBroker = calendarApiBroker;

        private async ValueTask<Calendar> PostRandomCalendarAsync()
        {
            Calendar randomCalendar = CreateRandomCalendar();
            await this.otripleSApiBroker.PostCalendarAsync(randomCalendar);

            return randomCalendar;
        }

        private Calendar CreateRandomCalendar() =>
             CreateRandomCalendarFiller().Create();

        private async ValueTask<Calendar> PostRandomGuardianAsync()
        {
            Calendar randomCalendar = CreateRandomCalendar();
            await this.otripleSApiBroker.PostCalendarAsync(randomCalendar);

            return randomCalendar;
        }

        private Calendar UpdateCalendarRandom(Calendar calendar)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Calendar>();

            filler.Setup()
                .OnProperty(calendar => calendar.Id).Use(calendar.Id)
                .OnProperty(calendar => calendar.CreatedBy).Use(calendar.CreatedBy)
                .OnProperty(calendar => calendar.UpdatedBy).Use(calendar.UpdatedBy)
                .OnProperty(calendar => calendar.CreatedDate).Use(calendar.CreatedDate)
                .OnProperty(calendar => calendar.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private Filler<Calendar> CreateRandomCalendarFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Calendar>();

            filler.Setup()
                .OnProperty(calendar => calendar.CreatedBy).Use(posterId)
                .OnProperty(calendar => calendar.UpdatedBy).Use(posterId)
                .OnProperty(calendar => calendar.CreatedDate).Use(now)
                .OnProperty(calendar => calendar.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
    }
}
