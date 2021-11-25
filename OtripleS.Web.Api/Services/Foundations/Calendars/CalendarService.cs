// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Calendars;

namespace OtripleS.Web.Api.Services.Foundations.Calendars
{
    public partial class CalendarService : ICalendarService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public CalendarService(IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Calendar> AddCalendarAsync(Calendar calendar) =>
        TryCatch(async () =>
        {
            ValidateCalendarOnCreate(calendar);

            return await this.storageBroker.InsertCalendarAsync(calendar);
        });

        public IQueryable<Calendar> RetrieveAllCalendars() =>
        TryCatch(() => this.storageBroker.SelectAllCalendars());

        public ValueTask<Calendar> RetrieveCalendarByIdAsync(Guid calendarId) =>
        TryCatch(async () =>
        {
            ValidateCalendarId(calendarId);
            Calendar maybeCalendar = await this.storageBroker.SelectCalendarByIdAsync(calendarId);
            ValidateStorageCalendar(maybeCalendar, calendarId);

            return maybeCalendar;
        });

        public ValueTask<Calendar> ModifyCalendarAsync(Calendar calendar) =>
        TryCatch(async () =>
        {
            ValidateCalendarOnModify(calendar);

            Calendar maybeCalendar =
               await this.storageBroker.SelectCalendarByIdAsync(calendar.Id);

            ValidateStorageCalendar(maybeCalendar, calendar.Id);

            ValidateAgainstStorageCalendarOnModify(
                inputCalendar: calendar,
                storageCalendar: maybeCalendar);

            return await this.storageBroker.UpdateCalendarAsync(calendar);
        });

        public ValueTask<Calendar> RemoveCalendarByIdAsync(Guid calendarId) =>
            TryCatch(async () =>
            {
                ValidateCalendarIdIsNull(calendarId);

                Calendar maybeCalendar =
                    await this.storageBroker.SelectCalendarByIdAsync(calendarId);

                ValidateStorageCalendar(maybeCalendar, calendarId);

                return await this.storageBroker.DeleteCalendarAsync(maybeCalendar);
            });
    }
}
