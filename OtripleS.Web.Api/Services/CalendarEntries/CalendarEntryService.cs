// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Models.CalendarEntries;

namespace OtripleS.Web.Api.Services.CalendarEntries
{
    public partial class CalendarEntryService : ICalendarEntryService
    {
		private readonly IStorageBroker storageBroker;
		private readonly ILoggingBroker loggingBroker;
		private readonly IDateTimeBroker dateTimeBroker;

		public CalendarEntryService(
			IStorageBroker storageBroker,
			ILoggingBroker loggingBroker,
			IDateTimeBroker dateTimeBroker)
		{
			this.storageBroker = storageBroker;
			this.loggingBroker = loggingBroker;
			this.dateTimeBroker = dateTimeBroker;
		}

		public IQueryable<CalendarEntry> RetrieveAllCalendarEntries() =>
		TryCatch(() =>
		{
			IQueryable<CalendarEntry> storageCalendarEntries = this.storageBroker.SelectAllCalendarEntries();

			ValidateStorageCalendarEntries(storageCalendarEntries);

			return storageCalendarEntries;
		});
    }
}
