using System;

namespace OtripleS.Web.Api.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTimeOffset GetCurrentDateTime() => DateTimeOffset.UtcNow;

        public DateTimeOffset GetRandomDate()
        {
            DateTime start = new DateTime(2000, 1, 1);
            Random rand = new Random();
            int range = (DateTime.Today - start).Days;

            return start.AddDays(rand.Next(range));
        }
    }
}
