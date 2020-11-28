// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Calenders
{
    [Collection(nameof(ApiTestCollection))]
    public partial class CalendersApiTest
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public CalendersApiTest(OtripleSApiBroker calenderApiBroker)
        {
            this.otripleSApiBroker = calenderApiBroker;
        }

        private async ValueTask<Calendar> PostRandomCalenderAsync()
        {
            Calendar randomCalender = CreateRandomCalender();
            await this.otripleSApiBroker.PostCalendarAsync(randomCalender);

            return randomCalender;
        }

        private IEnumerable<Calendar> GetRandomCalenders() =>
            this.CreateRandomCalenderFiller().Create(GetRandomNumber());

        private Calendar CreateRandomCalender() =>
            this.CreateRandomCalenderFiller().Create();

        private Filler<Calendar> CreateRandomCalenderFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Calendar>();

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
