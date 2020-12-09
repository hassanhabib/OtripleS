// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.CalendarEntries;

namespace OtripleS.Web.Api.Services.CalendarEntries
{
    public partial class CalendarEntryService : ICalendarEntryService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CalendarEntryService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<CalendarEntry> AddCalendarEntryAsync(CalendarEntry calendarEntry) =>
        TryCatch(async () =>
        {
            ValidateCalendarEntryOnCreate(calendarEntry);

            return await this.storageBroker.InsertCalendarEntryAsync(calendarEntry);
        });

        public ValueTask<CalendarEntry> ModifyCalendarEntryAsync(CalendarEntry calendarEntry) =>
        TryCatch(async () =>
        {
            ValidateCalendarEntryOnModify(calendarEntry);

            CalendarEntry maybeCalendarEntry =
               await this.storageBroker.SelectCalendarEntryByIdAsync(calendarEntry.Id);

            ValidateStorageCalendarEntry(maybeCalendarEntry, calendarEntry.Id);

            return await this.storageBroker.UpdateCalendarEntryAsync(calendarEntry);
        });

        public ValueTask<CalendarEntry> RemoveCalendarEntryByIdAsync(Guid calendarEntryId) =>
        TryCatch(async () =>
        {
            ValidateCalendarEntryId(calendarEntryId);

            CalendarEntry maybeCalendarEntry =
                await this.storageBroker.SelectCalendarEntryByIdAsync(calendarEntryId);

            ValidateStorageCalendarEntry(maybeCalendarEntry, calendarEntryId);

            return await this.storageBroker.DeleteCalendarEntryAsync(maybeCalendarEntry);
        });

        public IQueryable<CalendarEntry> RetrieveAllCalendarEntries() =>
        TryCatch(() =>
        {
            IQueryable<CalendarEntry> storageCalendarEntries =
                this.storageBroker.SelectAllCalendarEntries();

            ValidateStorageCalendarEntries(storageCalendarEntries);

            return storageCalendarEntries;
        });

        public ValueTask<CalendarEntry> RetrieveCalendarEntryByIdAsync(
            Guid calendarEntryId) =>
        TryCatch(async () =>
        {
            ValidateCalendarEntryId(calendarEntryId);

            CalendarEntry storageCalendarEntry =
                await this.storageBroker.SelectCalendarEntryByIdAsync(calendarEntryId);

            ValidateStorageCalendarEntry(storageCalendarEntry, calendarEntryId);

            return storageCalendarEntry;
        });
    }
}
