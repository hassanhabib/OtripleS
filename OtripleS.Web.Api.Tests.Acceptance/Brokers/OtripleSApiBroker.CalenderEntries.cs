// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.CalendarEntries;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string CalendarEntryRelativeUrl = "api/CalendarEntries";

        public async ValueTask<CalendarEntry> PostCalendarEntryAsync(CalendarEntry calendarEntry) =>
            await this.apiFactoryClient.PostContentAsync(CalendarEntryRelativeUrl, calendarEntry);

        public async ValueTask<CalendarEntry> GetCalendarEntryByIdAsync(Guid calendarEntryId) =>
            await this.apiFactoryClient.GetContentAsync<CalendarEntry>($"{CalendarEntryRelativeUrl}/{calendarEntryId}");

        public async ValueTask<CalendarEntry> PutCalendarEntryAsync(CalendarEntry calendarEntry) =>
            await this.apiFactoryClient.PutContentAsync(CalendarEntryRelativeUrl, calendarEntry);

        public async ValueTask<CalendarEntry> DeleteCalenderEntryByIdAsync(Guid calendarEntryId) =>
            await this.apiFactoryClient.DeleteContentAsync<CalendarEntry>($"{CalendarEntryRelativeUrl}/{calendarEntryId}");

        public async ValueTask<List<CalendarEntry>> GetAllCalendarEntriesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<CalendarEntry>>($"{CalendarEntryRelativeUrl}/");
    }
}
